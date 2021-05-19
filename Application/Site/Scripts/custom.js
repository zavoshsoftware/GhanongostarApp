
function addToBasket(code, qty) {
    if (qty === 'detail') {
        qty = $('#ddlQty').val();
    }
    $.ajax(
        {
            url: "/cart",
            data: { code: code, qty: qty },
            type: "Post"
        }).done(function (result) {
            if (result !== true) {
                window.location = "/basket";
            }
        });
}



function addDiscountCode() {
    var coupon = $("#coupon").val();

    $('#errorDiv').css('display', 'none');
    if (coupon !== "") {
        $.ajax(
            {
                url: "/shop/DiscountRequestPost",
                data: { coupon: coupon },
                type: "GET"
            }).done(function (result) {
                if (result !== "Invald" && result !== "Used" && result !== "Expired") {
                    location.reload();
                }
                else if (result !== true) {
                    $('#errorDiv').css('display', 'block');
                    if (result.toLowerCase() === "used") {
                        $('#errorDiv').html("این کد تخفیف قبلا استفاده شده است.");
                    }
                    else if (result.toLowerCase() === "expired") {
                        $('#errorDiv').html("کد تخفیف وارد شده منقضی شده است.");
                    }
                    else if (result.toLowerCase() === "invald") {
                        $('#errorDiv').html("کد تخفیف وارد شده معتبر نمی باشد.");
                    }
                    else if (result.toLowerCase() === "invalidproduct") {
                        $('#errorDiv').html("کد تخفیف وارد شده برای این محصول معتبر نمی باشد.");
                    }
                    else if (result.toLowerCase() === "true") {
                        $('#SuccessDiv').css('display', 'block');
                        $('#errorDiv').css('display', 'none');
                    }
                }
            });

    } else {
        $('#SuccessDiv').css('display', 'none');
        $('#errorDiv').html('کد تخفیف را وارد نمایید.');
        $('#errorDiv').css('display', 'block');
    }
}


function FinalizeOrder() {
    DisappearButton('btn-finalize', 'transfer-message2');
    var orderNotes = $('#orderNotes').val();
    var activationCode = $('#txtActivationCode').val();
    var cellNumber = $('#txtCellNum').val();
    var employeeType = $('#ddlEmployee').val();
    var email = $('#email').val();
    var city = $('#city').val();
    var address = $('#address').val();
    var postal = $('#postal').val();

    if (activationCode !== '') {
        $.ajax(
            {
                url: "/shop/Finalize",
                data: {
                    notes: orderNotes,
                    email: email,
                    activationCode: activationCode,
                    cellNumber: cellNumber,
                    employeeType: employeeType,
                    city: city,
                    address: address,
                    postal: postal
                },
                type: "GET"
            }).done(function (result) {

                if (result === "invalid") {
                    $('#error-box2').css('display', 'block');
                    $('#error-box2').html('کد وارد شده صحیح نمی باشد.');
                    AppearButton('btn-finalize', 'transfer-message2');
                }
                else if (result === "physical") {
                    $('.activate').css('display', 'none');
                    $('.register').css('display', 'block');
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('شهر و آدرس خود را وارد نمایید');
                    AppearButton('btn-register', 'transfer-message');
                    AppearButton('btn-finalize', 'transfer-message2');
                }

                else if (result !== "false"&&result!=="invalid") {
                    window.location = result;
                } else {
                    $('#error-box2').css('display', 'block');
                    $('#error-box2').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                    AppearButton('btn-finalize', 'transfer-message2');

                }
            });
    } else {
        $('#error-box2').css('display', 'block');
        $('#error-box2').html('لطفا اطلاعات بالا را تکمیل کنید');
        AppearButton('btn-finalize', 'transfer-message2');
    }
}

function registerUser() {
    DisappearButton('btn-register', 'transfer-message');
    var orderNotes = $('#orderNotes').val();
    var name = $('#txtFullName').val();
    var cellNumber = $('#txtCellNum').val();
    var employeeType = $('#ddlEmployee').val();
    var email = $('#email').val();
    var postalCode = $('#postal').val();
    var city = $('#city').val();
    var address = $('#address').val();
    var physical = $('.physical-product').val();
   var isPhysical= $('#isPhysical').val();

    if (isPhysical == 'false') {
        if (email !== '' && name !== '' && cellNumber !== '' && employeeType !== '') {
            $.ajax(
                {
                    url: "/shop/CheckUser",
                    data: {
                        notes: orderNotes,
                        email: email,
                        fullName: name,
                        cellNumber: cellNumber,
                        employeeType: employeeType 
                    },
                    type: "GET"
                }).done(function(result) {
                if (result === "true") {
                    $('.activate').css('display', 'block');
                    $('.register').css('display', 'none');
                } else if (result === "invalidMobile") {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('شماره موبایل وارد شده صحیح نمی باشد');
                    AppearButton('btn-register', 'transfer-message');
                } else if (result === "invalidEmail") {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('ایمیل وارد شده صحیح نمی باشد.');
                    AppearButton('btn-register', 'transfer-message');
                } else if (result === "invalidPostalCode") {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('کد پستی وارد شده صحیح نمی باشد.');
                    AppearButton('btn-register', 'transfer-message');
                } else {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                    AppearButton('btn-register', 'transfer-message');

                }
            });
        } else {
            $('#error-box').css('display', 'block');
            $('#error-box').html('لطفا فیلدهای ستاره دار را تکمیل کنید');
            AppearButton('btn-register', 'transfer-message');
        }
    } else {
        if (email !== '' && name !== '' && cellNumber !== '' && employeeType !== '' && postalCode !== '' && city !== '' && address!=='') {
            $.ajax(
                {
                    url: "/shop/CheckUser",
                    data: {
                        notes: orderNotes,
                        email: email,
                        fullName: name,
                        cellNumber: cellNumber,
                        employeeType: employeeType,
                        postalCode: postalCode
                    },
                    type: "GET"
                }).done(function (result) {
                    if (result === "true") {
                        $('.activate').css('display', 'block');
                        $('.register').css('display', 'none');
                    } else if (result === "invalidMobile") {
                        $('#error-box').css('display', 'block');
                        $('#error-box').html('شماره موبایل وارد شده صحیح نمی باشد');
                        AppearButton('btn-register', 'transfer-message');
                    } else if (result === "invalidEmail") {
                        $('#error-box').css('display', 'block');
                        $('#error-box').html('ایمیل وارد شده صحیح نمی باشد.');
                        AppearButton('btn-register', 'transfer-message');
                    } else if (result === "invalidPostalCode") {
                        $('#error-box').css('display', 'block');
                        $('#error-box').html('کد پستی وارد شده صحیح نمی باشد.');
                        AppearButton('btn-register', 'transfer-message');
                    } else {
                        $('#error-box').css('display', 'block');
                        $('#error-box').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                        AppearButton('btn-register', 'transfer-message');

                    }
                });
        } else {
            $('#error-box').css('display', 'block');
            $('#error-box').html('لطفا فیلدهای ستاره دار را تکمیل کنید');
            AppearButton('btn-register', 'transfer-message');
        }
    }
}



function DisappearButton(id, apearId) {
    $('#' + id).css('display', 'none');
    $('#' + apearId).css('display', 'block');

}
function AppearButton(id, apearId) {
    $('#' + id).css('display', 'block');
    $('#' + apearId).css('display', 'none');

}



function loginUser() {
    DisappearButton('btn-login', 'loading-box');
    
    var cellNumber = $('#txtCellNum').val();
   
    if (cellNumber !== '') {
        $.ajax(
            {
                url: "/account/SendOtp",
                data: {
                    cellNumber: cellNumber 
                },
                type: "GET"
            }).done(function (result) {

            if (result === "true") {

                DisappearButton('login-form', 'otp-form');

            } else if (result === "false"){

                $('#error-box-login').css('display', 'block');

                $('#error-box-login').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');

                AppearButton('btn-login', 'loading-box');

            } else if (result === "invalidUser"){
              
                DisappearButton('login-form', 'register-form');
                
            } else if (result === "invalidCellNumber"){

                $('#error-box-login').css('display', 'block');

                $('#error-box-login').html('شماره موبایل وارد شده صحیح نمی باشد ');

                AppearButton('btn-login', 'loading-box');

            }
        });
    } else {
        $('#error-box-login').css('display', 'block');
        $('#error-box-login').html('لطفا شماره موبایل خود را وارد نمایید');
        AppearButton('btn-login', 'loading-box');
    }
}

function CompleteRegisterFrom() {
    DisappearButton('btn-register', 'register-loading-box');
   
    var name = $('#fullName').val();
    var empType;
    if ($("#karfarma").prop("checked")) {
        empType = 'karfarma';
    } else {
        empType = 'karmand';
    }
    var cellNumber = $('#txtCellNum').val();
   

    if (name !== '' && cellNumber !== '') {
        $.ajax(
            {
                url: "/account/CompleteRegister",
                data: {
                    fullName: name,
                    cellNumber: cellNumber,
                    employeeType: empType
                },
                type: "GET"
            }).done(function (result) {
            if (result !== "false") {
                DisappearButton('register-form', 'otp-form');

            } else {
                $('#error-box-register').css('display', 'block');
                $('#error-box-register').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                AppearButton('btn-register', 'register-loading-box');

            }
        });
    } else {
        $('#error-box-register').css('display', 'block');
        $('#error-box-register').html('لطفا اطلاعات بالا را تکمیل کنید');
        AppearButton('btn-register', 'register-loading-box');
    }
}

function checkUserOtp() {
    DisappearButton('btn-checkOtp', 'activation-loading-box');
   
    var cellNumber = $('#txtCellNum').val();
    var code = $('#txtCode').val();

    if (code !== '' ) {
        $.ajax(
            {
                url: "/account/CheckOtp",
                data: {
                    activationCode: code,
                    cellNumber: cellNumber,
                },
                type: "GET"
            }).done(function (result) {
                if (result === "invalid") {
                    $('#error-box-otp').css('display', 'block');
                    $('#error-box-otp').html('کد وارد شده صحیح نمی باشد');
                    AppearButton('btn-checkOtp', 'activation-loading-box');

                }
                else if (result === "true") {
                var returnUrl = getUrlVars()["ReturnUrl"];
                if (returnUrl !== '' && returnUrl !=='undefined' && returnUrl !==undefined) {
                    window.location = returnUrl;

                } else {
                    window.location = "/orders/list";
                }
            } else {
                $('#error-box-otp').css('display', 'block');
                $('#error-box-otp').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                AppearButton('btn-checkOtp', 'activation-loading-box');

            }
        });
    } else {
        $('#error-box-otp').css('display', 'block');
        $('#error-box-otp').html('کد فعال سازی را وارد کنید');
        AppearButton('btn-checkOtp', 'activation-loading-box');
    }
}


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}