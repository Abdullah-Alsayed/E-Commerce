let arabic = "ar-EG";
let english = "en-US";

//Form Validation
const flatPickrEL = $(".flatpickr-validation");
if (flatPickrEL.length) {
  flatPickrEL.flatpickr({
    allowInput: true,
    monthSelectorType: "static"
  });
}

// Fetch all the forms we want to apply custom Bootstrap validation styles to
let bsValidationForms = document.querySelectorAll(".needs-validation");

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




function initializeDataTable(tableId, controller, action, columnsConfig, language, withAction = true) {

  const isArabic = language === arabic;
  if ($.fn.DataTable.isDataTable(`#${tableId}`)) {
    $(`#${tableId}`).DataTable().destroy();
  }

  // Find the index of the 'createAt' column dynamically
    let createAtIndex = tableColumns.findIndex(col => col.data === 'createAt');
  if (createAtIndex === -1) createAtIndex = 0; // Default to first column if not found


  $(`#${tableId}`).DataTable({
    language: {
      searchPlaceholder: isArabic ? "ابحث عن اي شيئ..." : 'Search...', // Custom search placeholder
      search: isArabic ? "البحث" : 'Search', // Remove default search text
      lengthMenu: isArabic ? "_MENU_ عدد العرض" : "Show _MENU_ entries", // Entries per page label
      paginate: {
          first: isArabic? "بداية" : "Start",
          previous: isArabic ? "السابق" : "Back",
          next: isArabic ? "التالي" : "Forward",
          last: isArabic ? "نهاية" : "End"
      },
          info: isArabic ? "_START_ إلى _END_ من _TOTAL_ مدخلات" : "Showing _START_ to _END_ of _TOTAL_ entries", // Information about entries
          infoEmpty: isArabic ? "لا توجد مدخلات" : "No entries available", // When no entries exist
          infoFiltered: isArabic ? "(مصفاة من _MAX_ مدخلات)" : "(filtered from _MAX_ total entries)", // Filtered info
    },
    processing: true,
    serverSide: true,
    search: {
      return: true
    },
    ajax: {
      url: `/${controller}/${action}`,
      type: 'POST',
      contentType: 'application/json',
        data: function (d) {
            const filters = getFilters(); // Collect all filters dynamically
            Object.assign(d, filters);   // Merge filters into the request payload
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
                 <div class="dropdown">
                    <button class="btn p-0" type="button" id="organicSessionsDropdown" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i class="mdi mdi-dots-vertical mdi-24px"></i>
                    </button>
                    <div class="dropdown-menu dropdown-menu-end" aria-labelledby="organicSessionsDropdown">
                        <a class="dropdown-item" onclick="fetchData('${controller}', 'Get', '${currentId}')" data-bs-toggle="modal" data-bs-target="#UpdateFormModal" href="javascript:void(0);">
                            ${isArabic ? 'تعديل' : 'Update'}
                        </a>
                        <a class="dropdown-item" onclick="setDeleteRecordId('${currentId}')" data-bs-toggle="modal" data-bs-target="#DeleteRecord" href="javascript:void(0);">
                            ${isArabic ? 'حذف' : 'Delete'}
                        </a>
                    </div>
                </div>
`               ;
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
  formData.append('id', updateRecordId);

  // Send data via AJAX
  $.ajax({
    url: `/${controler}/${action}`,
    type: action =="Update" ? 'PUT' : 'POST',
    data: formData,
    processData: false, // Prevent jQuery from processing the data
    contentType: false, // Prevent jQuery from setting the content type
    success: function (response) {
      if (response.isSuccess) {
          toastSuccess(response.message);

          if (action =="Create") {
            $(formSelector)[0].reset(); // Reset the form
            $(`${formSelector}Modal`).modal('hide'); // Hide the modal after success
            resetImagePreview(formSelector) // Reset the Photo
          }
          if ($.fn.DataTable.isDataTable(`#${table}`)) 
            $(`#${table}`).DataTable().ajax.reload();
      }
      else 
       toastError(response.message,5000);
      
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

            // If the input is a select element, handle it differently-
            if (input.is('select')) {
              HandelOptionCase(response, key, input);
            }
            else if (input.is('img')) {
              HandelImgeCase(response, pascalCaseKey, key);
            }
            else if (input.attr('type') === 'date' ||
                     input.attr('type') === 'datetime-local')
            {
              HandelDateCase(response, pascalCaseKey, key);
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

function updateIsActive(checkbox, controller) {
    let userId = checkbox.dataset.id;
    let isActive = checkbox.checked;

    $.ajax({
        url: `/${controller}/ToggleActive`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({ id: userId }),
        success: function (response) {
            if (response.isSuccess) {
                toastSuccess(response.message);
            } else {
                toastError(response.message);
                checkbox.checked = !isActive;
            }
        },
        error: function (ex) {
            checkbox.checked = !isActive;
            console.log(ex);
            toastError(ex.responseJSON.message);
        }
    });
}

// Attach event listener
$(document).on('change', '.switch-input', function () {
    let controller = window.location.pathname.split('/')[1];
    updateIsActive(this, controller);
});


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

function HandelDateCase(response, pascalKey, key) {
    const input = $('#UpdateForm').find(`[name="${pascalKey}"]`);
    const inputType = input.attr('type');
    const rawValue = response.result[key];

    if (!rawValue) return;

    // Ensure date is a valid Date object
    const date = new Date(convertUTCToLocal(rawValue));
    if (isNaN(date.getTime())) return; // Invalid date, skip

    let formatted = '';

    if (inputType === 'date') {
        formatted = date.toISOString().split('T')[0];
    } else if (inputType === 'datetime-local') {
        formatted = date.toISOString().slice(0, 16); // yyyy-MM-ddTHH:mm
    }

    input.val(formatted);
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

const getFilters = () => {
    const filters = {};
    $(`#filter-container .filter`).each(function () {
        const key = $(this).data('filter'); // Get the filter key from the data attribute
        let value = $(this).val();         // Get the selected value

        // Set default empty GUID if value is empty
        filters[key] = value ? value : "00000000-0000-0000-0000-000000000000";
    });
    return filters;
};


// Table Render
function convertUTCToLocal(utcDate) {
    if (!utcDate) return '-'; // Handle null/empty values

    let userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone; // Get user's time zone

    // Convert to UTC correctly by appending 'Z' if it's missing
    let date = new Date(utcDate.endsWith('Z') ? utcDate : utcDate + 'Z');

    // Convert UTC to local time using the user's time zone
    let localTime = date.toLocaleString(undefined, { timeZone: userTimeZone });

    return localTime;
}
function convertUTCToLocalDateOnly(utcDate) {
    if (!utcDate) return '-'; // Handle null/empty values

    const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    // Ensure UTC format
    const date = new Date(utcDate.endsWith('Z') ? utcDate : utcDate + 'Z');

    // Return date in M/D/YYYY format
    return date.toLocaleDateString('en-US', {
        timeZone: userTimeZone,
        year: 'numeric',
        month: 'numeric',
        day: 'numeric'
    });
}

function isActiveRender(data, row) {
    return `
        <label class="switch switch-primary">
            <input type="checkbox" class="switch-input" ${data ? 'checked' : ''} data-id="${row.id}" />
            <span class="switch-toggle-slider">
                <span class="switch-on"></span>
                <span class="switch-off"></span>
            </span>
        </label>
       `;
}

function entityRender(data) {
    let img = data.photoPath ? data.photoPath : '/Images/Default.png' ; 
    return `
        <div class="text-truncate entityRender fw-normal">
            <span class="avatar avatar-lg rounded-circle d-flex justify-content-center align-items-center">
                <img src="${img}" alt="Avatar" class="rounded-circle">
            </span>
            ${data.name}
        </div>
    `;
}

function photoRender(data) {
    return `<div class="avatar avatar-lg">
               <img src="${data}" alt="Avatar" class="rounded-circle">
           </div>
    `;
}

function userRender(data) {
    if (data != null)
    {
        let img = "";
        if (data.photo)
            img = `${data.photo}"`;
        else
            img = `/Images/User/@Constants.DefaultPhotos.User`;

        return `
            <div class="d-flex justify-content-center align-items-center customer-name">
                <div class="avatar-wrapper me-3">
                    <div class="avatar avatar-sm">
                        <img src="${img}" alt="Avatar" class="rounded-circle">
                    </div>
                </div>
                <div class="d-flex flex-column">
                    <a href="app-ecommerce-customer-details-overview.html" class="text-heading">
                        <span class="fw-medium text-truncate">${data.firstName} ${data.lastName ?? ""}</span>
                    </a>
                    <small class="text-nowrap">${data.email?? ""}</small>
                    <small class="text-nowrap">${data.phoneNumber?? ""}</small>
                </div>
            </div>
    `
    } else {
        return "-";
    }
}

function ratingRender(data) {
    let stars = '';
    for (let i = 0; i < data; i++) {
        stars += '<i class="mdi mdi-star gold-bg"></i>';
    }
    return stars;
}
