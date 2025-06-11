function updatePermission(id,action) {
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
      url: `/Role/${action}`,
    type: 'GET',
    dataType: 'html', // Ensure the server response is treated as HTML
    data: { id: id },
    success: function (response) {
      // Replace the card-body content with the received HTML
      $("#UpdatePermissionBody").html(response);
    },
    error: function (xhr, status, error) {
      // Handle errors
      let errorMessage = `
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

function submitPermissionForm(action) {
    let $button = $("#submitPermissionButton");
    let originalButtonContent = $button.html();

    // Add spinner to the button and disable it
    $button.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`);
    $button.prop("disabled", true);

    // Get RoleID from the hidden input
    let roleID = $("input[name='ID']").val();

    // Collect selected checkboxes
    let claims = [];
    $("input[name='Claims']:checked").each(function () {
        claims.push($(this).val());
    });

    // Create JSON object
    let requestData = {
        iD: roleID,
        claims: claims
    };
    console.log(requestData);
    // Perform the AJAX request
    $.ajax({
        url: `/Role/${action}`,
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

document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener("change", function (event) {
        // When any claim checkbox changes
        if (event.target.classList.contains("claim-checkbox")) {
            let groupIndex = event.target.getAttribute("data-group");
            updateSelectAllCheckbox(groupIndex);
        }

        // When "Select All" checkbox is clicked
        if (event.target.classList.contains("select-all-checkbox")) {
            toggleAllCheckboxes(event, event.target);
        }
    });
});

function toggleAllCheckboxes(event, selectAllCheckbox) {
    event.stopPropagation(); 

    let targetId = selectAllCheckbox.getAttribute("data-target");
    let checkboxes = document.querySelectorAll(targetId + " .claim-checkbox");

    checkboxes.forEach(function (checkbox) {
        checkbox.checked = selectAllCheckbox.checked;
    });
}

function updateSelectAllCheckbox(groupIndex) {
    let checkboxes = document.querySelectorAll(`[data-group="${groupIndex}"]`);
    let selectAllCheckbox = document.querySelector(`.select-all-checkbox[data-target="#collapse${groupIndex}"]`);

    if (checkboxes.length > 0) {
        let allChecked = Array.from(checkboxes).every(checkbox => checkbox.checked);
        selectAllCheckbox.checked = allChecked;
    }
}
