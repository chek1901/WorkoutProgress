$(document).ready(function () {

    $(function () {
        var placeholderElement = $('#modal-placeholder');

        $(document).on('click', 'button[data-toggle="ajax-modal"]', function () {
            var url = $(this).data('url');
            $.get(url).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            });
        });

        placeholderElement.on('click', '#createNewRole', function (event) {
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
                            position: 'middle',
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

    $(document).on('click', '.deleteRole', function (event) {
        event.preventDefault();

        var roleId = $(this).attr('data-delete-value');
        var roleName = $(this).attr('data-role-name');

        $.ajax({
            type: 'post',
            url: "/Admin/DeleteRole/" + roleId,
            contentType: 'application/json; charset=utf-8',
            dataType: 'text',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'role: ' + roleName + ' deleted successfully',
                    timer: 1500
                })
                getRoles();
            },
            error: function (xhr, status, error) {
                var errorText = xhr.status = "\r\n" + status + "\r\n" + error;
                console.log(errorText);
            },   
        });
    });


    function getRoles() {

        $.ajax({
            type: "get",
            url: "/Admin/ListRoles",
            contentType: "application/json; charset=utf-8",
            success: function () {
                console.log("loaded after delete function");
            },

        }).done(function (result) {
            $('#roleManage').html(result);
        });

    }
});