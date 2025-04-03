$(() => {
    $(".form-control").on('input', function () {
        ensureFormValidity();
    });

    function ensureFormValidity() {
        const isValid = isFormValid();
        $("#submit").prop('disabled', !isValid);
    }
    function isFormValid() {
        const name = $("#name").val();
        const content = $("#content").val();
       

        return name && content;
    }
})