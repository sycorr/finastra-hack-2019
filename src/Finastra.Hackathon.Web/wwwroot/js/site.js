var $ = jQuery;

$(document).ready(function () {
    // listen for sidebar clicks
    $(document).on('click',
        '#nav-toggle',
        function() {
            $('#sidebar').toggleClass('sidebar-display');
            $('.close-sidebar')
                .off('click')
                .on('click',
                    function () {
                        $('#sidebar').toggleClass('sidebar-display');
                    });
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
            $(this).popover('show');
        });

    // lender menu on start page
    $('.lender-menu li').on('click',
        function (e) {
            var name = $(this).attr('data-name');
            var username = $(this).attr('data-username');
            var id = $(this).attr('data-id');

            $('#user-dropdown').empty().append(`
                <div><img src="/img/portraits/${id}.jpg" max-width="32px"/>${name} (${username})</div>
            `);

            $('#username').val(username);
        });

    // start select 2 
    $('.s2').select2();

    $('.select2-container').addClass('form-control');
    $('.select2-selection').css('border', 'none');

    function dismissToast() {
        $('.toast-panel').addClass('d-none');
        // ajax to save dismissed stuff
        $.post('/toastDismissed',
            function() {

            });
    }

    // toast closing stuff
    $('.toast-close').on('click',
        function() {
            dismissToast();
        });

    // toast closing - temporary dummy button actions
    $('.toast-panel button').on('click',
        function() {
            dismissToast();
        });

    // inventory turnover calculations (accounting principles page)
    var cogs = +$('#costOfGoods').val();
    var inventory = +$('#inventory').val();
    var inventoryTurnover = cogs / inventory;
    $('#inventory-turnover').text(inventoryTurnover.toFixed(2));
    $('#inventory-turnover-days').text((+inventoryTurnover/360).toFixed(2));
});