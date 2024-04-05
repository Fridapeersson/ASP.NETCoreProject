console.log("HEJ");

const formErrorHandler = (element, validationResult) => {
    let spanElement = document.querySelector(`[data-valmsg-for="${element.name}"]`);
    //console.log(spanElement);
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

const textValidator = (element, minLength = 2) => {
    if (element.value.length >= minLength) {
        formErrorHandler(element, true);
    } else {
        formErrorHandler(element, false);
    }
};

const emailValidator = (element) => {
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    formErrorHandler(element, regex.test(element.value));
    console.log(element);
};

const passwordValidator = (element) => {
    if (element.dataset.valEqualtoOther !== undefined) {
        let password = document.getElementsByName(element.dataset.valEqualtoOther.replace("*", "Form"))[0].value
        if (element.value === password) {
            formErrorHandler(element, true);
        } else {
            formErrorHandler(element, false);
        }
    } else {
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_=+{};:,<.>]).{8,}$/;
        formErrorHandler(element, regex.test(element.value));
    }
}






const checkboxValidator = (element) => {
    formErrorHandler(element, element.checked);
};

let forms = document.querySelectorAll("form");
let inputs = forms[0].querySelectorAll("input");

inputs.forEach((input) => {
    if (input.dataset.val === "true") {
        if (input.type === "checkbox") {
            input.addEventListener("change", (e) => {
                checkboxValidator(e.target);
            });
        } else {
            input.addEventListener("keyup", (e) => {
                switch (e.target.type) {
                    case "text":
                        textValidator(e.target);
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

