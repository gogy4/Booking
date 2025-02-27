document.addEventListener("DOMContentLoaded", function () {
    let startDateInput = document.getElementById("startDate");
    let endDateInput = document.getElementById("endDate");
    let newPasswordInput = document.getElementById("NewPassword");
    let confirmPasswordInput = document.getElementById("ConfirmPassword");
    var bookedDates = window.bookedDates || [];

    if (startDateInput && endDateInput) {
        flatpickr("#startDate", {
            minDate: "today",
            maxDate: window.availableEndDate,
            disable: bookedDates,
            dateFormat: "Y-m-d",
            onChange: function () {
                adjustEndDate();
            }
        });

        flatpickr("#endDate", {
            minDate: "today",
            maxDate: window.availableEndDate,
            disable: bookedDates,
            dateFormat: "Y-m-d",
            defaultDate: endDateInput.value,
        });

        adjustEndDate();

        startDateInput.addEventListener("change", function () {
            endDateInput.min = startDateInput.value;
            if (endDateInput.value < startDateInput.value) {
                endDateInput.value = "";
            }
        });
    }

    if (newPasswordInput && confirmPasswordInput) {
        newPasswordInput.addEventListener("input", function () {
            confirmPasswordInput.required = this.value.trim() !== "";
        });
    }
});

function adjustEndDate() {
    var startDate = document.getElementById("startDate").value;
    if (startDate) {
        var start = new Date(startDate);
        start.setDate(start.getDate() + 1);
        var endDate = start.toISOString().split('T')[0];
        document.getElementById("endDate").value = endDate;
    }
}
