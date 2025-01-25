function Login(event) {
  event.preventDefault();  // Prevent the default form submission
  let form = $("#Login-Form")[0];
  if (form.checkValidity() === false) {
    form.classList.add('was-validated'); // Add validation feedback
    return; // Stop further execution if form is invalid
  }

  const LoginForm = document.querySelector('#Login-Form');

  const submitButton = LoginForm.querySelector('#Login-Btn');

  // Disable the submit button and show a spinner
  submitButton.disabled = true;
  const originalButtonText = submitButton.innerHTML;
  submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';

  // Gather form data
  //let formData = new FormData(LoginForm);
  const formData = $("#Login-Form").serialize();

  // Send AJAX request (or whatever action you need)
  $.ajax({
      url: '/Account/Login',
    type: 'POST',
    data: formData,
    success: function (result) {
      if (result.success) {
        location.replace("/");
      }
      else {
        // Re-enable the button and restore original text
        submitButton.disabled = false;
        submitButton.innerHTML = originalButtonText;
        toastError(result.message);  // Show error message
      }
    },
    error: function (xhr, status, error) {

      toastError(error);  // Show error message
    },
  });
}

function createUser(message, event) {
  confirmNewPasswordValidation(message,'CreateForm' ,event);
  submitFormData('#CreateForm', 'User', 'Create', 'UserList', event);
}

function confirmNewPasswordValidation(message,formId,event) {
  const form = document.getElementById(formId);

  event.preventDefault();
  // Validate Confirm Password
  const password = document.getElementById('NewPassword').value;
  const confirmPassword = document.getElementById('ConfirmNewPassword');

  if (password !== confirmPassword.value) {
    confirmPassword.setCustomValidity(message);
  } else {
    confirmPassword.setCustomValidity(""); // Clear any existing error
  }

  const formCheckValidity = form.checkValidity();

  // Check form validity
  if (!formCheckValidity) {
    form.classList.add('was-validated'); // Apply Bootstrap validation feedback
    return; // Exit if form is invalid
  }   
}

function changePassword(event) {

  event.preventDefault();

  // Ensure the form is valid before submitting
  let form = $("#changePasswordForm");
  let formSelector = form[0]; // Get the raw DOM element from the jQuery object

  if (formSelector.checkValidity() === false) {
    formSelector.classList.add('was-validated'); // Use vanilla JS to add the class
    return; // Stop further execution if form is invalid
  }

  let formData = form.serialize(); // Serialize form data

  const submitButton = $("#changePasswordBtn");

  // Disable the submit button and show a spinner
  submitButton.prop("disabled", true); // Use jQuery's .prop() method
  const originalButtonText = submitButton.html(); // Use jQuery's .html() method
  submitButton.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

  // Send AJAX request
  $.ajax({
    url: "/User/ChangePassword",
    type: "POST", // HTTP method
    data: formData, // Form data
    success: function (response) {
      // Handle success response
      if (response.success) {
        $("#ChangePasswordModal").modal("hide"); // Close the modal
        // Reset the form
        form[0].reset(); // Reset the form fields
        formSelector.classList.remove('was-validated'); // Remove validation feedback
        toastSuccess(response.message);
      } else {
        toastError(response.message); // Show error message from server
      }
    },
    error: function (xhr, status, error) {
      // Handle error
      toastError("An error occurred: " + error); // Show error message
    },
    complete: function()
    {
      // Re-enable the button and restore original text
      submitButton.prop("disabled", false);
      submitButton.html(originalButtonText);
    }
  });
}
