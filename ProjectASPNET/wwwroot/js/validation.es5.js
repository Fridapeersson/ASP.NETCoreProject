"use strict";

console.log("HEJ");

var formErrorHandler = function formErrorHandler(element, validationResult) {
    var spanElement = document.querySelector("[data-valmsg-for=\"" + element.name + "\"]");

    if (validationResult) {
        element.classList.remove("input-validation-error");
        spanElement.classList.remove("field-validation-error");
        spanElement.classList.add("field-validation-valid");
        spanElement.innerHTML = "";
    } else {
        element.classList.add("input-validation-error");
        spanElement.classList.add("field-validation-error");
        spanElement.classList.remove("field-validation-valid");
        spanElement.innerHTML = element.dataset.valRequired;
    }
};

var textValidator = function textValidator(element) {
    var minLength = arguments.length <= 1 || arguments[1] === undefined ? 2 : arguments[1];

    if (element.value.length >= minLength) {
        formErrorHandler(element, true);
    } else {
        formErrorHandler(element, false);
    }
};

var emailValidator = function emailValidator(element) {
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    formErrorHandler(element, regex.test(element.value));
};

var passwordValidator = function passwordValidator(element) {
    if (element.dataset.valEqualtoOther !== undefined) {
        var password = document.getElementsByName(element.dataset.valEqualtoOther.replace("*", "Form"))[0].value;
        if (element.value === password) {
            formErrorHandler(element, true);
        } else {
            formErrorHandler(element, false);
        }
    } else {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_=+{};:,<.>]).{8,}$/;
        formErrorHandler(element, regex.test(element.value));
    }
};

var checkboxValidator = function checkboxValidator(element) {
    formErrorHandler(element, element.checked);
};

var forms = document.querySelectorAll("form");
var inputs = forms[0].querySelectorAll("input");

inputs.forEach(function (input) {
    if (input.dataset.val === "true") {
        if (input.type === "checkbox") {
            input.addEventListener("change", function (e) {
                checkboxValidator(e.target);
            });
        } else {
            input.addEventListener("keyup", function (e) {
                switch (e.target.type) {
                    case "text":
                        textValidator(e.target);
                        console.log(input);
                        break;

                    case "email":
                        emailValidator(e.target);
                        break;
                    case "password":
                        passwordValidator(e.target);
                        break;
                }
            });
        }
    }
});

//account details address validation
document.addEventListener("DOMContentLoaded", function () {
    var addressForm = document.querySelector(".address-form-info");
    if (addressForm) {
        var _inputs = addressForm.querySelectorAll("input[type='text']");
        _inputs.forEach(function (input) {
            if (input.dataset.val === "true") {
                input.addEventListener("keyup", function (e) {
                    textValidator(e.target);
                });
            }
        });
    }
});

var deleteAccount = document.querySelector(".delete-account-form");
console.log(deleteAccount);
if (deleteAccount) {
    (function () {
        var checkbox = deleteAccount.querySelector("input[type = 'checkbox']");
        checkbox.addEventListener("change", function () {
            checkboxValidator(checkbox);
        });
    })();
}

document.addEventListener("DOMContentLoaded", function () {
    var contactForm = document.querySelector("#contact-form");
    console.log(contactForm);
    if (contactForm) {
        var _inputs2 = contactForm.querySelectorAll("input[type='textarea']");
        _inputs2.forEach(function (input) {
            if (input.dataset.val === "true") {
                input.addEventListener("keyup", function (e) {
                    textAreaValidator(e.target);
                });
            }
        });
    }
});

