$(document).ready(function () {

    $(function () {
        var placeholderElement = $('#modal-updateUser');

        $(document).on('click', 'button[data-toggle="ajax-modal-updateUser"]', function () {
            var url = $(this).data('url');
            var userId = $(this).attr('data-userId');
            $.get(url, {userId: userId}).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            });
        });
        placeholderElement.on('click', '#updateUser', function (event) {
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
                        url: "/Admin/ListUsers",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            console.log("It works");
                        }
                    }).done(function (result) {
                        $('#userManage').html(result);
                        Swal.fire({
                            position: 'middle',
                            icon: 'success',
                            title: 'User updated',
                            showConfirmButton: false,
                            timer: 1500
                        });

                    });

                }
            });
        });
      
               
    });

});