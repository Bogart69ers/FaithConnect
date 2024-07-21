document.addEventListener("DOMContentLoaded", function () {
    const discoverBtn = document.getElementById('discoverBtn');
    const myGroupsBtn = document.getElementById('myGroupsBtn');
    const discoverGroups = document.getElementById('discoverGroups');
    const myGroups = document.getElementById('myGroups');

    // Set the initial active tab and content
    discoverBtn.classList.add('active');
    discoverGroups.classList.add('active');

    discoverBtn.addEventListener('click', function () {
        discoverBtn.classList.add('active');
        myGroupsBtn.classList.remove('active');
        discoverGroups.classList.add('active');
        myGroups.classList.remove('active');
    });

    myGroupsBtn.addEventListener('click', function () {
        myGroupsBtn.classList.add('active');
        discoverBtn.classList.remove('active');
        myGroups.classList.add('active');
        discoverGroups.classList.remove('active');
    });

});
