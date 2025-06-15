/**
 * App eCommerce Add Product Script
 */
'use strict';

//Javascript to handle the e-commerce product add page

(function () {
  // Comment editor

  const commentEditor = document.querySelector('.comment-editor');

  if (commentEditor) {
    new Quill(commentEditor, {
      modules: {
        toolbar: '.comment-toolbar'
      },
      placeholder: 'Product Description',
      theme: 'snow'
    });
  }

  // previewTemplate: Updated Dropzone default previewTemplate

  // ! Don't change it unless you really know what you are doing

  const previewTemplate = `<div class="dz-preview dz-file-preview">
<div class="dz-details">
  <div class="dz-thumbnail">
    <img data-dz-thumbnail>
    <span class="dz-nopreview">No preview</span>
    <div class="dz-success-mark"></div>
    <div class="dz-error-mark"></div>
    <div class="dz-error-message"><span data-dz-errormessage></span></div>
    <div class="progress">
      <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" data-dz-uploadprogress></div>
    </div>
  </div>
  <div class="dz-filename" data-dz-name></div>
  <div class="dz-size" data-dz-size></div>
</div>
</div>`;

  // ? Start your code from here

  // Basic Dropzone

  const dropzoneBasic = document.querySelector('#dropzone-basic');
  if (dropzoneBasic) {
    const myDropzone = new Dropzone(dropzoneBasic, {
      previewTemplate: previewTemplate,
      parallelUploads: 1,
      maxFilesize: 5,
      acceptedFiles: '.jpg,.jpeg,.png,.gif',
      addRemoveLinks: true,
      maxFiles: 1
    });
  }

  // Basic Tags

  const tagifyBasicEl = document.querySelector('#ecommerce-product-tags');
  const TagifyBasic = new Tagify(tagifyBasicEl);

  // Flatpickr

  // Datepicker
  const date = new Date();

  const productDate = document.querySelector('.product-date');

  if (productDate) {
    productDate.flatpickr({
      monthSelectorType: 'static',
      defaultDate: date
    });
  }
})();

//Jquery to handle the e-commerce product add page

$(document).ready(function() {
  // Select2
  var select2 = $('.select2');
  if (select2.length) {
    select2.each(function () {
      var $this = $(this);
      select2Focus($this);
      $this.wrap('<div class="position-relative"></div>').select2({
        dropdownParent: $this.parent(),
        placeholder: $this.data('placeholder') // for dynamic placeholder
      });
    });
  }

  var formRepeater = $('.form-repeater');

  // Form Repeater
  // ! Using jQuery each loop to add dynamic id and class for inputs. You may need to improve it based on form fields.
  // -----------------------------------------------------------------------------------------------------------------

  if (formRepeater.length) {
    var row = 2;
    var col = 1;
    formRepeater.on('submit', function (e) {
      e.preventDefault();
    });
    formRepeater.repeater({
      show: function () {
        var fromControl = $(this).find('.form-control, .form-select');
        var formLabel = $(this).find('.form-label');

        fromControl.each(function (i) {
          var id = 'form-repeater-' + row + '-' + col;
          $(fromControl[i]).attr('id', id);
          $(formLabel[i]).attr('for', id);
          col++;
        });

        row++;
        $(this).slideDown();
        $('.select2-container').remove();
        $('.select2.form-select').select2({
          placeholder: 'Placeholder text'
        });
        $('.select2-container').css('width', '100%');
        var $this = $(this);
        select2Focus($this);
        $('.form-repeater:first .form-select').select2({
          dropdownParent: $(this).parent(),
          placeholder: 'Placeholder text'
        });
        $('.position-relative .select2').each(function () {
          $(this).select2({
            dropdownParent: $(this).closest('.position-relative')
          });
        });
      }
    });
  }

  

  
  
  // Initialize Select2 for both category and subcategory
  const categorySelect = $('#CategoryID');
  const subCategorySelect = $('#SubCategoryID');

  categorySelect.select2({
    placeholder: 'Select Category',
    allowClear: true,
    dropdownParent: categorySelect.parent()
  });

  subCategorySelect.select2({
    placeholder: 'Select Subcategory',
    allowClear: true,
    dropdownParent: subCategorySelect.parent()
  });

  // Add change event listener
  categorySelect.on('change', filterSubCategories);

  
  function filterSubCategories() {
    const selectedCategoryId = categorySelect.val();
    
    // Reset subcategory selection
    subCategorySelect.val(null).trigger('change');
    
    // Clear all existing options
    subCategorySelect.empty();
    
    const subCategoryContainer = $('#subCategoryContainer');
    
    if (selectedCategoryId) {
        // Make AJAX request to get subcategories
        $.ajax({
            url: '/SubCategory/GetAll',
            type: 'GET',
            data: {
                categoryId: selectedCategoryId,
                isActive: true
            },
            success: function(response) {
                if (response.isSuccess && response.result && response.result.items) {
                    const subCategories = response.result.items;
                    
                    // Add new options
                    subCategories.forEach(function(subCategory) {
                        subCategorySelect.append(`<option value="${subCategory.id}">${subCategory.name}</option>`);
                    });
                    
                    // Show container if we have subcategories
                    if (subCategories.length > 0) {
                        subCategoryContainer.show();
                        
                        // Reinitialize Select2 with placeholder
                        subCategorySelect.select2('destroy');
                        subCategorySelect.select2({
                            placeholder: "Select subcategory",
                            allowClear: true
                        });
                    } else {
                        subCategoryContainer.hide();
                    }
                } else {
                    subCategoryContainer.hide();
                }
            },
            error: function(xhr, status, error) {
                console.error('Error fetching subcategories:', error);
                subCategoryContainer.hide();
            }
        });
    } else {
        subCategoryContainer.hide();
    }
}
  // Initialize Tagify for tags input
  const input = document.querySelector('#TagIDs');
  const tags = document.querySelector('#tagsWhiteList').value || '';
  const whiteList = tags.split(',').map(tag => ({
      value: tag
  }));

  const tagify = new Tagify(input, {
      whitelist: whiteList, 
      maxTags: 100,
      dropdown: {
          maxItems: 100,
          classname: "tags-look",
          enabled: 0,
          closeOnSelect: false
      }
  });
  
});
