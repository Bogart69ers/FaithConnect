document.addEventListener("DOMContentLoaded", function () {
    const discoverBtn = document.getElementById('discoverBtn');
    const myGroupsBtn = document.getElementById('myGroupsBtn');
    const discoverGroups = document.getElementById('discoverGroups');
    const myGroups = document.getElementById('myGroups');
    const createGroup = document.getElementById('createGroup');
    const createGroupBtn = document.getElementById('createGroupBtn');

    
    

    // Set the initial active tab and content
    createGroup.classList.add('active');
    createGroupBtn.classList.add('active');

    discoverBtn.addEventListener('click', function () {
        discoverBtn.classList.add('active');
        myGroupsBtn.classList.remove('active');
        createGroupBtn.classList.remove('active');
        discoverGroups.classList.add('active');
        myGroups.classList.remove('active');
        createGroup.classList.remove('active');


    });

    myGroupsBtn.addEventListener('click', function () {
        myGroupsBtn.classList.add('active');
        discoverBtn.classList.remove('active');
        createGroupBtn.classList.remove('active');
        myGroups.classList.add('active');
        discoverGroups.classList.remove('active');
        createGroup.classList.remove('active');

    });

    createGroupBtn.addEventListener('click', function () {
        myGroupsBtn.classList.remove('active');
        discoverBtn.classList.remove('active');
        createGroupBtn.classList.add('active');
        myGroups.classList.remove('active');
        discoverGroups.classList.remove('active');
        createGroup.classList.add('active');

    });
});