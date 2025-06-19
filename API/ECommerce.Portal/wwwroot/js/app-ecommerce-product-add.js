'use strict';

Dropzone.autoDiscover = false;

$(document).ready(function () {

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
   })
};

    // -----------------------------
    // Dropzone for image upload
    // -----------------------------
    if (Dropzone.instances.length > 0)
        Dropzone.instances.forEach(z => z.destroy());

    const dropzoneBasic = document.querySelector('#dropzone-basic');
    if (dropzoneBasic) {
        const myDropzone = new Dropzone(dropzoneBasic, {
            paramName: "file",
            maxFilesize: 5,
            acceptedFiles: '.jpg,.jpeg,.png,.gif',
            addRemoveLinks: true,
            dictDefaultMessage: "Drag and drop your image here or click to upload",
            url: "/Product/UploadImage"
        });

        myDropzone.on("success", function (file, response) {
            const select = document.createElement("select");
            select.name = "ImageColors";
            select.className = "form-select mt-2";
            select.style.width = "90%";

            const defaultOption = document.createElement("option");
            defaultOption.value = "";
            defaultOption.text = "اختر اللون";
            select.appendChild(defaultOption);

            if (typeof colors !== "undefined" && Array.isArray(colors)) {
                colors.forEach(function (color) {
                    const option = document.createElement("option");
                    option.value = color.id;
                    option.text = color.name;
                    select.appendChild(option);
                });
            }

            file.previewElement.appendChild(select);
        });
    }

    // -----------------------------
    // Quill Editor for Description
    // -----------------------------

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
