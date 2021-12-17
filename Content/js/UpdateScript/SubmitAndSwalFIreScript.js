function confirmOrder(url, blockBody, formId) {
    KTApp.block(blockBody,
        {
            overlayColor: '#000000',
            type: 'v2',
            state: 'success',
            message: 'Please wait...'
        });

    $.ajax(
        {
            url: url,
            type: 'post',
            data: $(`form#${formId}`).serialize(),
            success: function (data) {
                KTApp.unblock(blockBody);
                if (data && data.success === true) {
                    Swal.fire({
                        title: "",
                        text: data.msg,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location = '/';
                        }
                    })
                }
                else {
                    toastr.error('Order isn\'t completed. Please try refresh the window.');
                }
            },
            error: function (data) {
                KTApp.unblock(blockBody);
                toastr.error('Information isn\'t send. Please try refresh the window.');
            }
        });
}
var KTFormControlsSwalFire = function () {
    // Private functions

    var demo1 = function (url, blockBody, formId) {
        $(`#${formId}`).validate({
            // define validation rules
            rules: {

            },
            errorPlacement: function (error, element) {
                var group = element.closest('.input-group');
                if (group.length) {
                    group.after(error.addClass('invalid-feedback'));
                } else {
                    element.after(error.addClass('invalid-feedback'));
                }
            },
            //display error alert on form submit
            invalidHandler: function (event, validator) {
                var alert = $('#kt_form_1_msg');
                alert.removeClass('kt--hide').show();
                KTUtil.scrollTop();
            },
            submitHandler: function (form) {
                //form[0].submit(); // submit the form
                window.confirmOrder(url, blockBody, formId);
            }
        });
    };
    return {
        // public functions
        init: function (url, blockBody, formId) {
            demo1(url, blockBody, formId);
        }
    };
}();