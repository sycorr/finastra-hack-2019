var $ = jQuery;

$(document).ready(function () {
    // listen for sidebar clicks
    $(document).on('click',
        '#nav-toggle',
        function() {
            $('#sidebar').toggleClass('sidebar-display');
        });

    // listen for sidebar clicks
    $(document).on('click',
        '#sidebar li',
        function () {
            $('#sidebar li').removeClass('active');
            $(this).addClass('active');
            var route = $(this).attr('data-route');
            window.location = '/' + route;
        });

    // listen for info-throbber clicks
    $('.info-icon').popover();
    $(document).on('click',
        '.info-icon',
        function () {

            console.log('showing popover');
            $(this).popover('show');

        });
});