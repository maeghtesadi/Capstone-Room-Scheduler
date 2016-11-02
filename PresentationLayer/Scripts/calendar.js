var funcCalled=false;
//Function is run when any of the timeslot li is clicked
$(".timeslots li ul li").click(function () {
    if (funcCalled == false) {
        var room = $(this).attr('data-room');
        var timeslot = $(this).attr('data-timeslot');
        funcCalled = true;
        this.classList.toggle("active");
        $(".reservation-popup h1").html("Select another timeslot");
        $(".reservation-popup").show(250);
        $(".timeslots li ul li").click(function () {
            if ($(this).attr('data-room') == room)
            {
                for(var i=parseInt(timeslot)+1;i<=parseInt($(this).attr('data-timeslot'));i++)
                {
                //You need to make find a jquery selector that selects an elemtn with [data-timeslot]=i and [data-room]=room
                    $("li[data-timeslot='" + i +"']").toggle("active");
                }
            }
           
     });
    }
   
    
});

//Function is run when cancel button is clicked
$(".btn-danger").click(function () {
    $(".reservation-popup").hide(250);
});

