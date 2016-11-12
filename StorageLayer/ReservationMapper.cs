using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLayer
{
    class ReservationMapper
    {


          public class ReservationtMapper
        {

            private static ReservationMapper instance = new ReservationMapper();

            private TDGReservation tdgReservation = TDGReservation.getInstance();
            private ClientIdentityMap clientIdentityMap = ClientIdentityMap.getInstance();

            private ClientMapper() { }

            public static ClientMapper getInstance()
            {
                return instance;
            }


            //Handles the Creation of a new object of type Client
            public Client makeNew(int id, String firstName, String lastName, double amount)
            {

                Client client = new Client(id, firstName, lastName, amount);

                //Add the new Client to the list of existing objects in Live memory
                clientIdentityMap.addTo(client);

                //Add to UoW registry so that we create it in the DB once the user is ready to commit everything.
                UnitOfWork.getInstance().registerNew(client);

                return client;
            }


            //Fetch Mesage for retrieving a Client with the ID.
            public Client getClient(int id)
            {

                Client client = clientIdentityMap.getInstance().find(id);
                Object result[] = null;

                // If Identity Map doesn't have it then use TDG.
                if (client == null)
                {
                    result = tdgClient.fetchClient(id);
                }

                // If TDG doesn't have it then it doens't exist.
                if (result == null)
                {
                    //Sorry no such client exist in the DB.
                    return null;
                }
                else
                {
                    //We got the client from the TDG who got it from the DB and now the mapper must add it to the ClientIdentityMap
                    client = new Client((int)result[0], (String)result[1], (String)result[2], (double)result[3]);
                    clientIdentityMap.getInstance().addTo(client);
                    return client;
                }

            }


            public void setClient(int id, double amount)
            {

                // First we fetch the client || We could have passed the Client as a Param. But this assumes you might not have
                // access to the instance of the desired object.
                Client client = getClient(id);

                // Mutator fuction to SET the new Amount.
                client.setAmount(amount);

                // We've modified something in the object so we Register the instance as Dirty in the UoW.
                UnitOfWork.getInstance().registerDirty(client);
            }


            public void delete(int id)
            {
                //Fire we fetch the client by checking the identity map
                Client cli = clientIdentityMap.find(id);

                // If the identity map returned the object, then remove it from the IdentityMap
                if (cli != null)
                {
                    ClientIdentityMap.getInstance().removeFrom(cli);
                }

                // We want to delete this object from out DB, so we simply register it as Deleted in the UoW
                UnitOfWork.getInstance().registerDeleted(cli);

            }

            // When we are ready to submit everything that is held in UnitofWork then we use done();
            public void done()
            {
                UnitOfWork.getInstance().commit();
            }



            //------------------------------------------------------------------------------------------------------------------
            // For Unit of Work to be able to call the appropriate functionalities.

            // Pass the list of Clients to add to DB to the TDG.
            public void addClient(LinkedList<Client> newList)
            {
                tdgClient.addClient(newList);
            }

            // Pass the list of Clients to update in the DB to the TDG
            public void updateClient(LinkedList<Client> updateList)
            {
                tdgClient.updateClient(updateList);
            }

            // Pass the list of Clients to remove from DB to the TDG
            public void deleteClient(LinkedList<Client> deleteList)
            {
                tdgClient.deleteClient(deleteList);
            }

        }













    }
}
