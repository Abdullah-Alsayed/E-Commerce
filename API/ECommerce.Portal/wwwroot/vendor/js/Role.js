function updatePermission(roleId) {
  const spinner = `
    <div class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
  `;

  // Show the spinner in the modal body before making the request
  $("#UpdatePermissionBody").html(spinner);

  // Perform the AJAX request
  $.ajax({
    url: '/Permission/ListAppServices', // Update this to your server endpoint
    type: 'GET',
    dataType: 'html', // Ensure the server response is treated as HTML
    data: { roleId: roleId },
    success: function (response) {
      // Replace the card-body content with the received HTML
      $("#UpdatePermissionBody").html(response);
    },
    error: function (xhr, status, error) {
      // Handle errors
      var errorMessage = `
        <div class="alert alert-danger" role="alert">
          Failed to load data: ${xhr.responseText || "An error occurred."}
        </div>
      `;
      $("#UpdatePermissionBody").html(errorMessage);
    },
    complete: function () {
      // Optional: Log or perform actions after the request completes
      console.log('Request completed.');
    }
  });
}
function submitPermissionForm() {
  // Get the button element
  var $button = $("#submitPermissionButton");

  // Save the original button content
  var originalButtonContent = $button.html();

  // Add spinner to the button and disable it
  $button.html(`
        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`);
  $button.prop("disabled", true);

  // Serialize the form data
  var formData = $("#updatePermissionForm").serialize();

  // Perform the AJAX request
  $.ajax({
    url: '/Permission/UpdateRoleAppService',
    type: 'POST',
    data: formData,
    success: function (response) {
      // Show success toast
      toastSuccess(response.message);

      // Restore the button state
      $button.html(originalButtonContent);
      $button.prop("disabled", false);
    },
    error: function (xhr, status, error) {
      // Show error toast
      toastError("An error occurred while saving permissions.");

      // Restore the button state
      $button.html(originalButtonContent);
      $button.prop("disabled", false);
    }
  });
}

