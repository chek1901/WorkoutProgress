$(document).ready(function () {

    $('.logout').on('click', function () {
        console.log('clicked logout Btn');
        var userName = $('.logoutUser').val();

        Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, logout!',


        }).then((result) => {
            if (result.isConfirmed) {
                $.post({
                    url: "/Account/Logout",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        console.log("Successfully posted to logout method");
                    }
                }).done(function (result) {
                    let timerInterval;
                    Swal.fire({
                        title: 'Loggin out ' + userName ,
                        html: 'I will close in <b></b> milliseconds.',
                        timer: 2000,
                        timerProgressBar: true,
                        willOpen: () => {
                            Swal.showLoading()
                            timerInterval = setInterval(() => {
                                const content = Swal.getContent()
                                if (content) {
                                    const b = content.querySelector('b')
                                    if (b) {
                                        b.textContent = Swal.getTimerLeft()
                                    }
                                }
                            }, 100)
                        },
                        onClose: () => {
                            clearInterval(timerInterval)
                            location.reload(true);
                        }
                    }).then((result) => {
                        /* Read more about handling dismissals below */
                        if (result.dismiss === Swal.DismissReason.timer) {
                            console.log('I was closed by the timer')
                        }
                    })
                });
            }
        })
    });
    

});