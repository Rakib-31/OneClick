"use strict";

// Class Definition
var KTLoginV1 = function () {
	var login = $('#kt_login');

	var showErrorMsg = function(form, type, msg) {
        var alert = $('<div class="alert alert-bold alert-solid-' + type + ' alert-dismissible" role="alert">\
			<div class="alert-text">'+msg+'</div>\
			<div class="alert-close">\
                <i class="flaticon2-cross kt-icon-sm" data-dismiss="alert"></i>\
            </div>\
		</div>');

        form.find('.alert').remove();
        alert.prependTo(form);
        KTUtil.animateClass(alert[0], 'fadeIn animated');
    }

	// Private Functions
	var handleSignInFormSubmit = function () {
		$('#kt_login_signin_submit').click(function (e) {
			e.preventDefault();

			var btn = $(this);
			var form = $('#kt_login_form');

			form.validate({
				rules: {
					username: {
						required: true
					},
					password: {
						required: true
					}
				}
			});

			if (!form.valid()) {
				return;
			}

			KTApp.progress(btn[0]);

			setTimeout(function () {
				KTApp.unprogress(btn[0]);
			}, 2000);
            debugger;
			// ajax form submit:  http://jquery.malsup.com/form/
			form.ajaxSubmit({
                url: '/Account/Login',
				success: function (data) {
                   
					// similate 2s delay
					KTApp.unprogress(btn[0]);
                    if (data.success === true) {
                        //  toastr.success(data.Message);
                    }
                    else if (data.success === false) {
                        var msg = data.Message;
                        if (msg == "" || msg == undefined) {
                            msg = "Unknown Error.";
                        }
                        toastr.error(msg);
                    }
                    if (data.RedirectUrl !== "") {
                        window.location.href = data.RedirectUrl;
                    }
				}
			});
		});
	}

	// Public Functions
	return {
		// public functions
		init: function () {
			handleSignInFormSubmit();
		}
	};
}();

// Class Initialization
jQuery(document).ready(function () {
	KTLoginV1.init();
});
