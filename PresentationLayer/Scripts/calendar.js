function addActiveClass(element) {
    element.classList.toggle("active");
    $(".reservation-popup").show(250);
}
$(".btn-danger").click(function () {
    $(".reservation-popup").hide(250);


});