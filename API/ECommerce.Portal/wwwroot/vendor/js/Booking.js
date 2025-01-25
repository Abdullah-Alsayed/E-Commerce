function PayBooking(controller, action, table) {
  // Disable the button to prevent multiple clicks
  const payButton = $('#payBookingButton'); // Assuming the button has an ID of 'payBookingButton'
  payButton.prop('disabled', true);

  const recordId = $('#PayBookingId').val();

  $.ajax({
    url: `/${controller}/${action}`,
    type: 'POST',
    data: { bookingId: recordId },
    success: function (response) {
      if (response.success) {
        toastSuccess(response.message); // Display success message
        $('#PayBooking').modal('hide'); // Hide modal
        $(`#${table}`).DataTable().ajax.reload(); // Reload DataTable
      } else {
        toastError(response.message); // Display error message
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to pay for the booking.';
      toastError(errorMessage); // Display error message from the response
    },
    complete: function () {
      // Re-enable the button after the request is complete (success or error)
      payButton.prop('disabled', false);
    }
  });
}

function CancelBooking(controller, action, table) {
  // Disable the button to prevent multiple clicks
  const cancelButton = $('#cancelBookingButton'); // Assuming the button has an ID of 'cancelBookingButton'
  cancelButton.prop('disabled', true);

  const recordId = $('#CancelBookingId').val();

  $.ajax({
    url: `/${controller}/${action}`,
    type: 'PUT',
    data: { id: recordId },
    success: function (response) {
      if (response.success) {
        toastSuccess(response.message); // Display success message
        $('#CancelBooking').modal('hide'); // Hide modal
        $(`#${table}`).DataTable().ajax.reload(); // Reload DataTable
      } else {
        toastError(response.message); // Display error message
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      const errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Failed to cancel the record.';
      toastError(errorMessage); // Display error message from the response
    },
    complete: function () {
      // Re-enable the button after the request is complete (success or error)
      cancelButton.prop('disabled', false);
    }
  });
}

function UpdateBookingDate(controller, action, table) {
  // Disable the button to prevent multiple clicks
  const updateButton = $('#updateDateButton'); // Assuming the button has an ID of 'updateDateButton'
  updateButton.prop('disabled', true);

  const bookingId = $('#UpdateBookingDateId').val();
  const newDate = $('#NewBookingDate').val();

  $.ajax({
    url: `/${controller}/${action}`,
    type: 'PUT',
    data: { id: bookingId, date: newDate },
    success: function (response) {
      if (response.success) {
        toastSuccess(response.message); // Display success message
        $('#UpdateDateModal').modal('hide'); // Hide modal
        $(`#${table}`).DataTable().ajax.reload(); // Reload DataTable
      } else {
        toastError(response.message); // Display error message from the response
      }
    },
    error: function (xhr, status, error) {
      console.error('AJAX Error:', error);
      let errorMessage = 'Failed to update the booking date.';
      if (xhr.responseJSON && xhr.responseJSON.message) {
        errorMessage = xhr.responseJSON.message; // Use server's error message
      }
      toastError(errorMessage); // Display error message
    },
    complete: function () {
      // Re-enable the button after the request is complete (success or error)
      updateButton.prop('disabled', false);
      $('#NewBookingDate').val(''); // Clear the new date input
    }
  });
}
