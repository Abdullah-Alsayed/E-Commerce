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
      url: '/Role/GetClaims',
    type: 'GET',
    dataType: 'html', // Ensure the server response is treated as HTML
    data: { id: roleId },
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
    let $button = $("#submitPermissionButton");
    let originalButtonContent = $button.html();

    // Add spinner to the button and disable it
    $button.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`);
    $button.prop("disabled", true);

    // Get RoleID from the hidden input
    let roleID = $("input[name='RoleID']").val();

    // Collect selected checkboxes
    let claims = [];
    $("input[name='Claims']:checked").each(function () {
        claims.push($(this).val());
    });

    // Create JSON object
    let requestData = {
        roleID: roleID,
        claims: claims
    };
    // Perform the AJAX request
    $.ajax({
        url: '/Role/UpdateRoleClaims',
        type: 'PUT',
        data: requestData, 
        success: function (response) {
            if (response.isSuccess)
                toastSuccess(response.message);
            else
            toastError(response.message);
        },
        error: function (xhr, status, error) {
            toastError("An error occurred while saving permissions.");
        },
        complete: function () {
            // Restore the button state
            $button.html(originalButtonContent);
            $button.prop("disabled", false);
        }
    });
}




