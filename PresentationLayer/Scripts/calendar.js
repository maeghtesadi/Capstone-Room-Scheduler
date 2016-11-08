var funcCalled = false;
var seconfuncCalled = false;
var firstAndLastTimeslot = [0,0];
var confirmRservation = $(".reservation-popup h1").html();
//Function is run when any of the timeslot li is clicked
$(".timeslots li ul li").click(function () {
    if (funcCalled == false) {
        var thisElement = this;
        var room = $(this).data("room");
        var timeslot = $(this).data("timeslot");
        firstAndLastTimeslot[0] = timeslot;
        funcCalled = true;
        this.classList.toggle("active");
        $(".reservation-popup-test h1").html("Select another timeslot");
        //$(".reservation-popup-test").show(1);
        $(".reservation-popup-test").position({
            my: "left top",
            at:"right top",
            of: thisElement,
            collision:"fit"
        
        });
        
        $(".timeslots li ul li").click(function () {
            var timeslot2 = $(this).data("timeslot");

            if (seconfuncCalled == false) {
                firstAndLastTimeslot[1] = $(this).data("timeslot") - 1;
                seconfuncCalled = true;
            }
            //if timeslot selected at the begining or after the range
            if ($(this).attr('data-room') == room) {
                if (firstAndLastTimeslot[1] < timeslot2) {
                    for (var i = parseInt(timeslot) + 1; i <= parseInt($(this).attr('data-timeslot')) ; i++) {
                    
                        //adds more timeslots to the already active tismeslots
                        if ($("li[data-timeslot='" + i + "']li[data-room='" + room + "']").hasClass("active")) { }
                        else {
                            //toggles active the desired timesots
                            $("li[data-timeslot='" + i + "']li[data-room='" + room + "']").toggleClass("active");
                        }
                    }
                    
                    firstAndLastTimeslot[1] = timeslot2;
                }   
                else {
                    //if timeslot is selected before the range
                    if (firstAndLastTimeslot[0] > timeslot2) {
                        timeslot = timeslot2;
                        firstAndLastTimeslot[0] = timeslot2;
                        for (var i = firstAndLastTimeslot[0]; i <= firstAndLastTimeslot[1] ; i++) {

                            //adds more timeslots to the already active tismeslots
                            if ($("li[data-timeslot='" + i + "']li[data-room='" + room + "']").hasClass("active")) { }
                            else {
                                //toggles active the desired timesots
                                $("li[data-timeslot='" + i + "']li[data-room='" + room + "']").toggleClass("active");
                            }
                        }


                        
                    }
                    //if timeslot selected is between the range that was already selected
                    else {
                        for (var i = parseInt(timeslot2) + 1; i <= parseInt(firstAndLastTimeslot[1]) ; i++) {
                            $("li[data-timeslot='" + i + "']li[data-room='" + room + "']").toggleClass("active");
                        }
                        firstAndLastTimeslot[1] = timeslot2;
                    }

                }
                

                //firstAndLastTimeslot[1] = $(this).data("timeslot");
                $(".reservation-popup h1").html(confirmRservation);
            }
            
        });
    }


});

//Function is run when cancel button is clicked
$(".btn-danger").click(function () {
    $(".reservation-popup").hide(250);
});