$(document).ready(function () {

    $(document).on('submit', '#userInfoForm', function (event) {
        event.preventDefault();
        var placeholder = $('#userInfoDiv');
        var form = $('#userInfoForm');
        var urlAction = form.attr('action');
        var dataToSend = form.serialize();

        $.post(urlAction, dataToSend).done(function (data) {
            var newBody = $('#userInfoForm', data);
            placeholder.find('form').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';

            if (isValid) {
                console.log("its valid");
                Swal.fire({
                    position: 'middle',
                    icon: 'success',
                    title: 'User updated',
                    showConfirmButton: true
                });
            }
            else
            {
                Swal.fire({
                    position: 'middle',
                    icon: 'error',
                    title: 'User not updated',
                    showConfirmButton: true
                });
                console.log("not valid");
            }
        });
       
    });


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
                        dataType: 'json',
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