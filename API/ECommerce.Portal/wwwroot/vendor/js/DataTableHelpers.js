let arabic = "Arabic";
let english = "English";

//Form Validation
const flatPickrEL = $(".flatpickr-validation");
if (flatPickrEL.length) {
  flatPickrEL.flatpickr({
    allowInput: true,
    monthSelectorType: "static"
  });
}

// Fetch all the forms we want to apply custom Bootstrap validation styles to
var bsValidationForms = document.querySelectorAll(".needs-validation");

// Loop over them and prevent submission
Array.prototype.slice.call(bsValidationForms).forEach(function (form) {
  form.addEventListener(
    "submit",
    function (event) {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      } else {
        // Submit your form
        alert("Submitted!!!");
      }

      form.classList.add("was-validated");
    },
    false
  );
});




function initializeDataTable(tableId, controler, action, columnsConfig, language, withAction = true) {

  if ($.fn.DataTable.isDataTable(`#${tableId}`)) {
    $(`#${tableId}`).DataTable().destroy();
  }

  const filterId = $(`#${tableId}`).attr('data-filterId');
  // Find the index of the 'createAt' column dynamically
  let createAtIndex = tableColumns.findIndex(col => col.data === 'createAt');
  if (createAtIndex === -1) createAtIndex = 0; // Default to first column if not found

  $(`#${tableId}`).DataTable({
    language: {
      searchPlaceholder: language == arabic ? "ابحث عن اي شيئ..." : 'Search...', // Custom search placeholder
      search: language == arabic ? "البحث" : 'Search', // Remove default search text
      lengthMenu: language == arabic ? "_MENU_ عدد العرض" : "Show _MENU_ entries", // Entries per page label
      paginate: {
        first: language == arabic ? "بداية" : "Start",
        previous: language == arabic ? "السابق" : "Back",
        next: language == arabic ? "التالي" : "Forward",
        last: language == arabic ? "نهاية" : "End"
      },
      info: language == arabic ? "_START_ إلى _END_ من _TOTAL_ مدخلات" : "Showing _START_ to _END_ of _TOTAL_ entries", // Information about entries
      infoEmpty: language == arabic ? "لا توجد مدخلات" : "No entries available", // When no entries exist
      infoFiltered: language == arabic ? "(مصفاة من _MAX_ مدخلات)" : "(filtered from _MAX_ total entries)", // Filtered info
    },
    processing: true,
    serverSide: true,
    search: {
      return: true
    },
    ajax: {
      url: `/${controler}/${action}?id=${filterId}`,
      type: 'POST',
      contentType: 'application/json',
      data: function (d) {
        let data = JSON.stringify(d);
        console.log(data);
        return data;
      },
      error: function (xhr, status, error) {
        console.error('Error:', error);
        const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to delete the record.';
        toastError(errorMessage); // Display error message from the response
      },
    },
    columns: withAction ? [
      ...columnsConfig,
      {
        data: null, // No specific data field for buttons
        orderable: false, // Disable ordering for this column
        searchable: false, // Disable searching for this column
        render: function (data, type, row) {
          // Generate Add and Delete buttons
          let currentId = row[`id`];
          return `
          <div class="d-flex">
              <button type="button" onclick=fetchData('${controler}','Get','${currentId}') class="btn btn-icon me-2 btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#UpdateFormModal">
                 <span class="mdi mdi-square-edit-outline"></span>
              </button>
              <button type="button" onclick=setDeleteRecordId('${currentId}') class="btn btn-icon me-2 btn-danger btn-sm" data-bs-toggle="modal" data-bs-target="#DeleteRecord">
                 <span class="mdi mdi-delete-forever"></span>
              </button>
            </div>`;
        }
      }
    ] : [...columnsConfig],
      order: [[createAtIndex, 'desc']]
  });
}


//Command 
function submitFormData(formSelector, controler, action, table, e) {
  // Handle form submission
  e.preventDefault(); // Prevent the default form submission

  // Ensure the form is valid before submitting
  let form = $(formSelector)[0];
  if (form.checkValidity() === false) {
    form.classList.add('was-validated'); // Add validation feedback
    return; // Stop further execution if form is invalid
  }

  // Get the file input inside the form
  let fileInput = $(formSelector).find('input[type="file"]')[0];
  let file = fileInput?.files[0];

  // Validate the image file if present
  if (file && !validateImageFile(file)) {
    return; // Stop if the file is invalid
  }

  // Get the submit button inside the form
  let submitBtn = $(formSelector).find('button[type="submit"]');

  // Disable the button and show spinner
  submitBtn.prop('disabled', true);
  let originalBtnContent = submitBtn.html(); // Save the original content
  submitBtn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

  // Create FormData for submission
  let formData = new FormData(form);
  if (file) {
    formData.append('image', file); // Append the image file
  }

  const updateRecordId = $(`#updateRecordId`).val();

  // Send data via AJAX
  $.ajax({
    url: `/${controler}/${action}?id=${updateRecordId}`,
    type: 'POST',
    data: formData,
    processData: false, // Prevent jQuery from processing the data
    contentType: false, // Prevent jQuery from setting the content type
    success: function (response) {
      if (response.isSuccess) {
        toastSuccess(response.message);
        $(formSelector)[0].reset(); // Reset the form
        $(`${formSelector}Modal`).modal('hide'); // Hide the modal after success
        resetImagePreview(formSelector) // Reset the Photo
        $(`#${table}`).DataTable().ajax.reload(); // Reload DataTable
      } else {
        toastError(response.message);
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to submit the record.';
      toastError(errorMessage); 
    },
    complete: function () {
      // Re-enable the button and restore its content
      submitBtn.prop('disabled', false);
      submitBtn.html(originalBtnContent);
    }
  });
}

function DeleteRecord(controller, action, table) {
  // Disable the button to prevent multiple clicks
  const deleteButton = $('#deleteRecordButton'); // Assuming the button has an ID of 'deleteRecordButton'
  deleteButton.prop('disabled', true);

  const recordId = $('#deleteRecordId').val();

  $.ajax({
    url: `/${controller}/${action}`,
    type: 'DELETE',
    data: { id: recordId },
    success: function (response) {
      if (response.isSuccess) {
        toastSuccess(response.message); // Display success message
        $('#DeleteRecord').modal('hide'); // Hide modal
        $(`#${table}`).DataTable().ajax.reload(); // Reload DataTable
      } else {
        toastError(response.message); // Display error message
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to delete the record.';
      toastError(errorMessage); // Display error message from the response
    },
    complete: function () {
      // Re-enable the button after the request is complete (success or error)
      deleteButton.prop('disabled', false);
    }
  });
}

function fetchData(controller, action, id) {
  $('#UpdateForm-spinner').removeClass('d-none'); // Hide the spinner
  $('#UpdateForm').addClass('d-none'); // Show the form
  // Send AJAX request to get data

  setUpdateRecordId(id);

  $.ajax({
    url: `/${controller}/${action}/${id}`,
    type: 'GET',
    dataType: 'json',
    success: function (response) {
      if (response.isSuccess) {
        // Populate the form with data
        for (let key in response.result) {
          let pascalCaseKey = toPascalCase(key); // Convert key to PascalCase
          let input = $('#UpdateForm').find(`[name="${pascalCaseKey}"]`);
          if (input.length) {

            // If the input is a select element, handle it differently
            if (input.is('select')) {
              HandelOptionCase(response, key, input);
            }
            else if (input.is('img')) {
              HandelImgeCase(response, pascalCaseKey, key);
            }
            else {
              input.val(response.result[key]);
            }
          }
        }
      } else {
        toastError(response.message);
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to Get the record.';
      toastError(errorMessage); // Display error message from the response
    },
    complete: function () {
      // Hide the spinner and show the form after the request completes
      $('#UpdateForm-spinner').addClass('d-none'); // Hide the spinner
      $('#UpdateForm').removeClass('d-none'); // Show the form
    }
  });
}

function updateImagePreview(formName, event) {
  const fileInput = event.target;
  const file = fileInput.files[0];
  const imageElement = $(`#${formName} img`); // Select the image with jQuery

  if (file) {
    const reader = new FileReader();
    reader.onload = function (e) {
      imageElement.attr('src', e.target.result);
      imageElement.removeClass("d-none");
      if (formName =="CreateForm") {
      $(`#Img-MainBox`).addClass("d-none");
      }
    };
    reader.readAsDataURL(file); // Read the file as a data URL
  } else {
    imageElement.attr('src', ''); // Use .attr() to reset the src
  }
}

function resetImagePreview(formName) {
  const imageElement = $(`${formName} img`); // Select the image
  const fileInput = $(`${formName} input[type="file"]`); // Select the file input

  // Reset the file input and image source
  fileInput.val('');
  imageElement.attr('src', '');
  imageElement.addClass('d-none');

  // Remove the "d-none" class from the main box
  if (formName === "#CreateForm") {
    $(`#Img-MainBox`).removeClass("d-none");
  }
}

  

//Helpers
function setDeleteRecordId(recordId) {
  $(`#deleteRecordId`).val(recordId);
}

function setUpdateRecordId(recordId) {
  $(`#updateRecordId`).val(recordId);
}

function setIdValue(id,value) {
  $(`#${id}`).val(value);
}

function toPascalCase(str) {
  return str
    .replace(/(^\w|_\w)/g, (match) => match.replace('_', '').toUpperCase());
}

function HandelOptionCase(response, key, input) {
  let valueToSelect = response.result[key];
  let options = input.find('option');

  // Loop through the options and set the matching option as selected
  options.each(function () {
    if ($(this).val() == valueToSelect.toString()) {
      $(this).prop('selected', true);
    }
  });

  // Trigger change event
  input.trigger('change');
}

function HandelImgeCase(response, pascalCaseKey, key) {
  let imagePreview = $(`#UpdateForm #${pascalCaseKey}`);
  if (imagePreview.length && response.result[key]) {
    imagePreview.prop('src', response.result[key]);
  } else {
    imagePreview.attr('src', '/path-to-default-placeholder.png');
  }
}

// Private method to validate image file
function validateImageFile(file) {
  // Validate the file type (e.g., only allow images)
  const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
  if (!validImageTypes.includes(file.type)) {
    toastError('Invalid file type. Please upload an image.');
    return false;
  }

  // Validate file size (e.g., max 5MB)
  const maxFileSize = 5 * 1024 * 1024; // 5 MB
  if (file.size > maxFileSize) {
    toastError('File is too large. Maximum size is 5MB.');
    return false;
  }

  return true; // Valid file
} 
