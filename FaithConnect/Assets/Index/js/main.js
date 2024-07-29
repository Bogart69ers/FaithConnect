//// popover
//var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
//var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
//    return new bootstrap.Popover(popoverTriggerEl);
//});

//// Gender Select
//if (window.location.pathname === "/") {
//    const radioBtn1 = document.querySelector("#flexRadioDefault1");
//    const radioBtn2 = document.querySelector("#flexRadioDefault2");
//    const radioBtn3 = document.querySelector("#flexRadioDefault3");
//    const genderSelect = document.querySelector("#genderSelect");

//    radioBtn1.addEventListener("change", () => {
//        genderSelect.classList.add("d-none");
//    });
//    radioBtn2.addEventListener("change", () => {
//        genderSelect.classList.add("d-none");
//    });
//    radioBtn3.addEventListener("change", () => {
//        genderSelect.classList.remove("d-none");
//    });
//}

document.addEventListener("DOMContentLoaded", function () {
    // Initial setup
    const currentMonthEl = document.getElementById('currentMonth');
    const prevMonthBtn = document.getElementById('prevMonth');
    const nextMonthBtn = document.getElementById('nextMonth');
    const upcomingEventsBtn = document.getElementById('upcomingEventsBtn');
    const recentEventsBtn = document.getElementById('recentEventsBtn');
    const upcomingEvents = document.getElementById('upcomingEvents');
    const recentEvents = document.getElementById('recentEvents');

    let currentDate = new Date();
    let currentMonth = currentDate.getMonth();
    let currentYear = currentDate.getFullYear();

    function renderCalendar() {
        const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate();
        const firstDay = new Date(currentYear, currentMonth, 1).getDay();
        const calendarBody = document.querySelector('.calendar-container tbody');
        const eventDates = Array.from(document.querySelectorAll('.event-item')).map(item => new Date(item.getAttribute('data-date')));

        calendarBody.innerHTML = '';
        let date = 1;

        for (let i = 0; i < 6; i++) {
            const row = document.createElement('tr');

            for (let j = 0; j < 7; j++) {
                const cell = document.createElement('td');
                if (i === 0 && j < firstDay) {
                    cell.appendChild(document.createTextNode(''));
                } else if (date > daysInMonth) {
                    break;
                } else {
                    const span = document.createElement('span');
                    span.textContent = date;
                    cell.appendChild(span);
                    const cellDate = new Date(currentYear, currentMonth, date);
                    
                    if (eventDates.some(eventDate => eventDate.getDate() === date && eventDate.getMonth() === currentMonth && eventDate.getFullYear() === currentYear)) {
                        cell.classList.add('event-day');
                    }
                    date++;
                }
                row.appendChild(cell);
            }
            calendarBody.appendChild(row);
        }
        currentMonthEl.textContent = new Date(currentYear, currentMonth).toLocaleString('default', { month: 'long', year: 'numeric' });
    }

    prevMonthBtn.addEventListener('click', function () {
        currentMonth = currentMonth === 0 ? 11 : currentMonth - 1;
        currentYear = currentMonth === 11 ? currentYear - 1 : currentYear;
        renderCalendar();
    });

    nextMonthBtn.addEventListener('click', function () {
        currentMonth = currentMonth === 11 ? 0 : currentMonth + 1;
        currentYear = currentMonth === 0 ? currentYear + 1 : currentYear;
        renderCalendar();
    });

    upcomingEventsBtn.addEventListener('click', function () {
        upcomingEvents.classList.remove('d-none');
        recentEvents.classList.add('d-none');
        upcomingEventsBtn.classList.add('active');
        recentEventsBtn.classList.remove('active');
    });

    recentEventsBtn.addEventListener('click', function () {
        upcomingEvents.classList.add('d-none');
        recentEvents.classList.remove('d-none');
        upcomingEventsBtn.classList.remove('active');
        recentEventsBtn.classList.add('active');
    });

    renderCalendar();
});
