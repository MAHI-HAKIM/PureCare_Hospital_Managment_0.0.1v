//Scroll-Feature for a stickybar
const navE1 = document.querySelector('.navbar');

window.addEventListener('scroll', () => {
    if (window.scrollY >= 180) {
        navE1.classList.add('navbar-scrolled');
        navE1.classList.add('fixed-top');

    } else if (window.scrollY < 180) {
        //navE1.classList.remove('navbar-scrolled');
        navE1.classList.remove('fixed-top');

    }
});


//SELECTING DOCTOR ACCORDING TO THEIR DEPARTMENT

//$(document).ready(function () {
//    $('#DepartmentId').change(function () {
//        var selectedDepartment = $(this).val();
//        if (selectedDepartment) {
//            $.ajax({
//                url: '/AppointmentController/GetDoctorsByDepartment',
//                type: 'GET',
//                data: { department: selectedDepartment },
//                success: function (data) {
//                    // Populate the doctor dropdown with the returned data
//                    $('#DoctorId').html(data);
//                    // Show the doctor dropdown container
//                    $('#doctorDropdownContainer').show();
//                },
//                error: function () {
//                    console.log('Error fetching doctors.');
//                }
//            });
//        } else {
//            // If no department selected, hide the doctor dropdown container
//            $('#doctorDropdownContainer').hide();
//        }
//    });
//});