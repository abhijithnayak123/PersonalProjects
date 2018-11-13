$(function () {
    $('.navbar-toggle').click(function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
        $('#search').removeClass('in').addClass('collapse').slideUp(200);
        /// uncomment code for absolute positioning tweek see top comment in css
        //$('.absolute-wrapper').toggleClass('slide-in');
        
    });

     $('.sidebar-toggle').click(function () {
        $('.menu-row').toggleClass('shortMenu');
        // $('.side-menu').toggleClass('shortMenu');
        // $('.top-menu').toggleClass('shortMenu');
        // $('.container-fluid').toggleClass('shortMenu');
    });

     $("li.panel").attr("closed", "true");
     var checkif = $("li.panel").attr("closed");
     
     $('li.panel a').click(function () {

         if (checkif == "true") {
             $("li.panel").attr("closed", "false");
             $('li.panel a').find('i.oi').removeClass('oi-caret-right').addClass('oi-caret-bottom');
         }
         if (checkif == "false") {
             $("li.panel").attr("closed", "true");
             $('li.panel a').find('i.oi').removeClass('oi-caret-bottom').addClass('oi-caret-right');
         }

     }); 
});

// $(window).load(function() {
//     function pageloader(){
//         $(".loader").delay( 3000 ).fadeOut("slow");
//     }
//     pageloader();
// });