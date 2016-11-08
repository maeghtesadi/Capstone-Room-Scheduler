﻿var funcCalled = false;
var confirmRservation = $(".reservation-popup h1").html();
//Function is run when any of the timeslot li is clicked
$(".timeslots li ul li").click(function () {
    if (funcCalled == false) {
        var thisElement = this;
        var room = $(this).attr('data-room');
        var timeslot = $(this).attr('data-timeslot');
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
            if ($(this).attr('data-room') == room) {
                for (var i = parseInt(timeslot) + 1; i <= parseInt($(this).attr('data-timeslot')) ; i++) {
                    //You need to make find a jquery selector that selects an elemtn with [data-timeslot]=i and [data-room]=room
                    $("li[data-timeslot='" + i + "']li[data-room='" + room + "']").toggleClass("active");
                   
                }
                $(".reservation-popup h1").html(confirmRservation);
            }

        });
    }


});

//Function is run when cancel button is clicked
$(".btn-danger").click(function () {
    $(".reservation-popup").hide(250);
});