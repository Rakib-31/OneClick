function orderPreSubmit(url, blockBody, dataId) {
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
            type: 'get',
            data: $(`form#${dataId}`).serialize(),
            success: function (data) {
                KTApp.unblock(blockBody);
            },
            error: function (data) {
                console.log(data);
                KTApp.unblock(blockBody);
                toastr.error('Information isn\'t send. Please try refresh the window.');
            }
        });
}
var KTFormControls = function () {
    // Private functions

    var demo1 = function (url, blockBody, dataId) {
        $(`#${dataId}`).validate({
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
                window.orderPreSubmit(url, blockBody, dataId);
            }
        });
    };
    return {
        // public functions
        init: function (url, blockBody, dataId) {
            demo1(url, blockBody, dataId);
        }
    };
}();