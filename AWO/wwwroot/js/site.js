jQuery(function ($) {

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        // save the latest tab; use cookies if you like 'em better:
        sessionStorage.setItem('lastTab', $(e.target).attr('href'));
    });

    var lastTab = sessionStorage.getItem('lastTab');
    if (lastTab) {
        $('[href="' + lastTab + '"]').tab('show');
    }

    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if (
            $(this)
                .parent()
                .hasClass("active")
        ) {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .parent()
                .removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .next(".sidebar-submenu")
                .slideDown(200);
            $(this)
                .parent()
                .addClass("active");
        }
    });

    $("#close-sidebar").click(function () {
        $(".page-wrapper").removeClass("toggled");
    });
    $("#show-sidebar").click(function () {
        $(".page-wrapper").addClass("toggled");
    });


    $('.myBtn').click((e) => {
        e.preventDefault();
        Swal.fire({
            title: 'Error!',
            text: 'Do you want to continue',
            icon: 'error',
            confirmButtonText: 'Cool'
        });

    });



    $(function () {
        var placeholderElement = $('#modal-placeholder');

        //$(document).on('click', 'button[data-toggle="ajax-modal"]', function() )
        $(document).on('click', 'button[data-toggle="ajax-modal"]', function () {
            var url = $(this).data('url');
            $.get(url).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            });
        });

        placeholderElement.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();

            var form = $(this).parents('.modal').find('form');
            var actionUrl = form.attr('action');
            var dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                var newBody = $('.modal-body', data);
                placeholderElement.find('.modal-body').replaceWith(newBody);

                var isValid = newBody.find('[name="IsValid"]').val() == 'True';

                if (isValid) {
                    placeholderElement.find('.modal').modal('hide');

                    $.ajax({
                        type: "get",
                        url: "/Admin/ListRoles",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            console.log("It works");

                        }
                    }).done(function (result) {
                        $('#roleManage').html(result);
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Role created',
                            showConfirmButton: false,
                            timer: 1500
                        });

                    });

                }
            });
        });
    });

    $(document).on('click', '.roleDelete', function () {
        var value = $(this)
    });

});   