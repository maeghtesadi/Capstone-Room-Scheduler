//reservedTimeslots color pallette
var colorPallette = [
    ['#16a085', '#1abc9c'],
    ['#27ae60', '#2ecc71'],
    ['#2980b9', '#3498db'],
    ['#8e44ad', '#9b59b6'],
    ['#2c3e50', '#34495e'],
    ['#f39c12', '#f1c40f'],
    ['#d35400', '#e67e22'],
    ['#c0392b', '#e74c3c'],
    ['#7f8c8d', '#95a5a6']

];

//Function is run when any of the timeslot li is clicked
function timeslotClicked(event) {
    var seconfuncCalled = false;
    var firstAndLastTimeslot = [0, 0];
    var thisElement = event;
    var room = $(event).data("room");
    $("input[name='room']").attr("value", room);
    var timeslot = $(event).data("timeslot");
    firstAndLastTimeslot[0] = timeslot;
    $("#firstTimeslot").html(firstAndLastTimeslot[0]);
    $("#lastTimeslot").html(firstAndLastTimeslot[0] + 1);
    funcCalled = true;
    $(event).addClass("active");
    $(".reservation-popup-test h1").html("Select another timeslot");
    $(".reservation-popup-test").toggle(0);
    $(".reservation-popup-test").css('opacity', '0');
    $(".reservation-popup-test").position({
        my: "left top",
        at: "right+7 top+-7",
        of: thisElement,
    });
    $(".reservation-popup-test").toggle(0);
    $(".reservation-popup-test").css('opacity', '1');
    $(".reservation-popup-test").toggle(300);
    $(".timeslots li ul li").off("click.firstFunction");
    $(".timeslots li ul li").on("click.secondFunction", function () {

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
                    for (var i = firstAndLastTimeslot[0]; i <= firstAndLastTimeslot[1]; i++) {

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

        }
        $("#firstTimeslot").html(firstAndLastTimeslot[0]);
        $("input[name='firstTimeslot']").attr("value", firstAndLastTimeslot[0]);

        $("#lastTimeslot").html(firstAndLastTimeslot[1] + 1);
        $("input[name='lastTimeslot']").attr("value", firstAndLastTimeslot[1]);

    });



}
$(".timeslots li ul li").on("click.firstFunction", function () {
    timeslotClicked(this);
});

//Function is run when cancel button is clicked
$(".reservation-popup-test .header span").click(function () {
    $(".reservation-popup-test").toggle(250);
    $(".timeslots li ul li").off("click.secondFunction");
    $(".timeslots li ul li").on("click.firstFunction", function () {
        timeslotClicked(this);
    });
    $(".timeslots .active").toggleClass("active");

});

//Get reservation info from the server to populate the timeslots
$.connection.hub.start().done(function () {
    serverSession.server.updateCalendar();
});
var serverSession = $.connection.calendarHub;
//Jquery to update the timeslots

serverSession.client.getReservations = function (reservationList) {
    for (j = 0; j < reservationList.length; j++) {
        var color = colorPallette[Math.floor(Math.random() * colorPallette.length)];
        for (var i = reservationList[j].initialTimeslot; i <= reservationList[j].finalTimeslot; i++) {
            $("li[data-timeslot='" + i + "']li[data-room='" + reservationList[j].roomId + "']").addClass("reserved");
            $("li[data-timeslot='" + i + "']li[data-room='" + reservationList[j].roomId + "']").html("");
            $("li[data-timeslot='" + i + "']li[data-room='" + reservationList[j].roomId + "']").css('background-color',color[1]);
        }
        //First timeslot classtoggle=reservedHeader
        $("li[data-timeslot='" + (reservationList[j].initialTimeslot) + "']li[data-room='" + reservationList[j].roomId + "']").addClass("reserved-header").html(reservationList[j].studentName);
        $("li[data-timeslot='" + (reservationList[j].initialTimeslot) + "']li[data-room='" + reservationList[j].roomId + "']").css('background-color', color[0]);
        //Second timeslot classtoggle=reservedd;
        var time = "<u>Time</u>: From " + reservationList[j].initialTimeslot + " to " + (parseInt(reservationList[j].finalTimeslot) + 1);
        var courseName = "<u>Course Name</u>: " + reservationList[j].courseName;
        var waitingList = "<u>Waiting List:</u>:";
        $("li[data-timeslot='" + (reservationList[j].initialTimeslot + 1) + "']li[data-room='" + reservationList[j].roomId + "']").html(time + "</br>" + courseName + "</br>" + waitingList);
        
    }
    $(".glyphicon-remove").click();

};

//Login popup
$(".dropdownLogin").click(function () {
    $(".login-popup").toggle(300);
});
$(".userId").click(function () {
    $(".login-popup").toggle(300);
    $("#username").remove();
    $("#password").remove();
    $("#failedMessage").remove();
    $("#loginButton").html("Log Out");
});

function OnSuccess(data) {
    if (data.responseText != "Success") {
        $("#failedMessage").html("Invalid credentials");
    }
    else {
        location.reload();
    }

}


function buildNewReservationItem(reservationId, description, initialTimeSlot, finalTimeslot , roomID ) //reservtion id goes in .$(".cancelReservation).data(reservationId)
{
    var reservationItem = 
        '<div class="reservation-item">' +
             '<div class="roomNumber"><span class="fa fa-stack fa-lg"><i class="fa fa-circle fa-stack-1x"></i><i class="fa fa-stack number">' + roomID + '</i></span></div>' +
             '<div class="reservationDetails">'+
                '<ul>'+
                '    <li class="description">'+ description +'</li>'+
                '    <li class="timeslot">From <span class="initialTimeslot">' + initialTimeSlot + '</span> to <span class="finalTimeslot">' + finalTimeslot + '</span></li>' +
                '</u>' +
             '</div>' +
             '<div data-reservationId ="' + reservationId + '"    class="cancelReservation"><span class="fa fa-pencil fa-lg"></span></div>' +
        '</div>';

    $(".reservations .reservation-content ").append(reservationItem);
}

function getrReservationsFromDB() {


}
