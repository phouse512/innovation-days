/*
    loadUserListener is called by places where user info is displayed in a table, displayed by User/Search
        inputs   - style: either "normal" or "inverted" to signify the color scheme for the selected user in the 
                                table (normal -- white, other -- light blue)
        function - determines what style to use for the 'selected' class
                 - adds listeners for all <tr> tags and chooses the styling based on current choices

    Dependencies - JQuery
*/
function loadUserListener(style) {
    if (style == "normal") {
        selectedUserClass = 'selectedUser';
        selectedJQueryClass = '.selectedUser';
    } else {
        selectedUserClass = 'selectedUserInverted';
        selectedJQueryClass = '.selectedUserInverted';
    }
    console.log('setting');
    $(".selectuser").on("click", "tr", function () {
        if ($(this).hasClass(selectedUserClass)) {
            $(selectedJQueryClass).removeClass(selectedUserClass);
        } else if ($(selectedJQueryClass)[0]) {
            console.log('another');
            $(selectedJQueryClass).removeClass(selectedUserClass);
            $(this).addClass(selectedUserClass);
        } else {
            console.log(selectedUserClass);
            $(this).removeClass();
            $(this).addClass(selectedUserClass);
        }
    });
}

function loadUserSearchResultsListener(root) {
    $("#displaySearch").on("click", "tr", function () {
        document.location.href = 'User/Details/' + $(this).attr('id');
    });
}

/*
    editProjectLeader is called by a listener on the "Details" page for projects
        inputs   - leaderSelect: the int type of what project leader is being selected
                 - url: the url to be called by the ajax call in search (default: User/Search)
        function - loads the modal for selecting the project leaders
                 - customizes modal depending on selection
                 - adds listeners for the search buttons 
                 - displays search results in a table
*/
function editProjectLeader(leaderSelect, url) {
    var buttonTitle;
    var buttonRemoveTitle;
    
    switch (leaderSelect) {
        case "idea":
            buttonTitle = "Select Idea Owner";
            buttonRemove = "Remove Current Owner";
            break;
        case "lead":
            buttonTitle = "Select Team Lead";
            buttonRemove = "Remove Current Lead";
            break;
        case "presenter":
            buttonTitle = "Select Presenter";
            buttonRemove = "Remove Current Presenter";
            break;
    }
    var submit = $("#submitLeadChange");
    $(submit).html(buttonTitle);

    $("#removeLead").html(buttonRemove);

    $("#modalSearchUsers").click(function () {
        searchUsersModal($("#searchInput").val(), url);
    });
    $("#searchInput").keypress(function (event) {
        if (event.keyCode == 13) {
            searchUsersModal($("#searchInput").val(), url);
        }
    });
}

function autoLoadUsers(url) {
    searchUsersModal("", url);
}

function searchAllUsers(searchString, searchURL, root) {
    $('#waitImage').show();
    searchURL += "?searchTerm=" + searchString + "&modalWindow=" + 0;

    $.ajax({
        url: searchURL,
        type: 'GET',
        success: function (data) {
            $("#displaySearch").html(data);
            $("#displaySearch").off("click");
            loadUserSearchResultsListener(root);
            $('#waitImage').hide('slow');
        },
        error: function (request, status, err) {
            alert(request + " " + status);
        }
    });
}

/* 
    Search Users Function and helper - returns a table of users contained in search for a modal window
    Input - takes the search 
*/
function searchUsersModal(searchString, searchURL) {
    //var searchParameters = returnSearchCriteria(searchString, searchType);
    //var jsonData = JSON.stringify(searchParameters, null, 2);

    searchURL += "?searchTerm=" + searchString + "&modalWindow=" + 1;
    console.log(searchURL);

    $("#waitImage").show();

    $.ajax({
        url: searchURL,
        type: 'GET',
        success: function (data) {
            $(".selectuser").html(data);
            $(".selectuser").off("click");
            loadUserListener("hi");
            $("#waitImage").hide('slow');
        },
        error: function(request, status, err) {
            alert(status + " " + err);
        }
    });
}

// Returns a searchCritera json model object
function returnSearchCriteria(searchField, searchType) {
    return {
        searchString: searchField,
        searchType: searchType
    };
}

function submitLeadButton(userId, editorName){
    var originalUrl = $("#modifyURL").val();
    var projectId = $("#projectID").val();
    var modifyType;

    switch($("#submitLeadChange").html()){
        case "Select Idea Owner":
            modifyType = 1;
            break;
        case "Select Team Lead":
            modifyType = 0;
            break;
        case "Select Presenter":
            modifyType = 2;
            break;
    }
    originalUrl += "/" + projectId + "/" + modifyType + "/" + userId + "/" + editorName;
    window.location = originalUrl;
}


//Search projects data
function loadProjectSearchResultsListener() {
    $("#displaySearch").on("click", "tr", function () {
        document.location.href = 'Project/Details/' + $(this).attr('id');
    });
}

function loadPaginationListener(currentPage, searchString, searchURL, pageSize) {
    $("#paginationBar").on("click", "li", function () {
        if ($(this).hasClass("disabled")) {

        } else if ($(this).hasClass("previousPage")) {
            searchAllProjects(searchString, searchURL, (currentPage-1), pageSize);
        } else if ($(this).hasClass("nextPage")) {
            console.log(currentPage + 1);
            searchAllProjects(searchString, searchURL, (currentPage + 1), pageSize);
        } else {
            var val = parseInt($(this).children().text());
            searchAllProjects(searchString, searchURL, val, pageSize);
        }
    });
}

function searchAllProjects(searchString, searchURL, requestedPage, pageSize) {
    $('#waitImage').show();
    data = {
        searchTerm: searchString,
        requestedPage: requestedPage,
        PageSize: pageSize
    };

    $.ajax({
        url: searchURL,
        type: 'POST',
        data: data,
        success: function (data) {
            //parse html return into pagination as well as table display
            var split = data.split("<hr />");
            $("#displaySearch").html(split[0]);
            $("#displaySearch").remove("ul");
            $("#pagination").html(split[1]);
            $("tr", "#pagination").remove();

            $("#displaySearch").off("click");
            $("#paginationBar").off("click");
            loadProjectSearchResultsListener();
            loadPaginationListener(requestedPage, searchString, searchURL, pageSize);
            $('#waitImage').hide('slow');
        },
        error: function (request, status, err) {
            alert(request + " " + status);
        }
    });
}

function returnPaginatedEditHistory(projectId, editURL, requestedPage, pageSize) {
    $("#waitImage").show('slow', function () {
        $("return").hide('slow'); 
    });
    data = {
        projectid: projectId,
        requestedPage: requestedPage,
        PageSize: pageSize
    };

    $.ajax({
        url: editURL,
        type: 'POST',
        data: data,
        success: function (data) {
            $("#results").html(data);
            $("#paginationBar").off("click");
            loadHistoryPaginationListener(requestedPage, projectId, editURL, pageSize);
            loadViewSnapshotListener();
            $('#waitImage').hide('slow', function () {
                $('#return').show('slow');
            });
        },
        error: function (request, status, err) {
            alert(request + " " + status);
        }
    });
}

function loadHistoryPaginationListener(currentPage, searchString, searchURL, pageSize) {
    $("#paginationBar").on("click", "li", function () {
        if ($(this).hasClass("disabled")) {

        } else if ($(this).hasClass("previousPage")) {
            returnPaginatedEditHistory(searchString, searchURL, (currentPage - 1), pageSize);
        } else if ($(this).hasClass("nextPage")) {
            console.log(currentPage + 1);
            returnPaginatedEditHistory(searchString, searchURL, (currentPage + 1), pageSize);
        } else {
            var val = parseInt($(this).children().text());
            returnPaginatedEditHistory(searchString, searchURL, val, pageSize);
        }
    });
}

function loadViewSnapshotListener() {
    $(".panel-group").on("click", ".btn-small", function () {
        var url = "../../Project/ViewSnapshot/" + $(this).attr('id');
        document.location.href = url;
    });
}

function getUpdatedPosts(lastId, updateURL, projectid) {
    data = {
        lastLogId: lastId,
        projectid: projectid
    };

    $.ajax({
        url: updateURL,
        type: 'POST',
        data: data,
        success: function (data) {
            var target = $("#posts");
            $($.parseHTML(data)).attr("jquery_workaround", "sigh").hide().appendTo(target);
            var newStuff = target.find("[jquery_workaround=sigh]");
            newStuff.removeAttr("jquery_workaround").slideDown("slow", function () {
                if (data != "") {
                    $("#posts").animate({ scrollTop: $('#posts')[0].scrollHeight }, 3000);
                }
            });
        },
        error: function (request, status, err) {
            alert(request + " " + status);
        }
    });
}

function createPost(editorName, parentProject, content, createURL, updateURL) {
    //editorName = editorName.replace("MRISOFTWARE", "MRISOFTWARE\\");
    data = {
        posterName: editorName,
        parentProject: parentProject,
        content: content
    };


    $.ajax({
        url: createURL,
        type: 'POST',
        data: data,
        success: function (data) {
            lastId = $(".panel", "#posts").last().attr("id");
            if (typeof lastId == 'undefined') {
                lastId = 0;
            }
            getUpdatedPosts(lastId, updateURL, parentProject);
        },
        error: function (request, status, err) {
            alert(request + " " + status);
        }
    });

}

function autoUpdatePosts(updateURL, projectid) {
    lastId = $(".panel", "#posts").last().attr("id");
    if (typeof lastId == 'undefined') {
        lastId = 0;
    }
    getUpdatedPosts(lastId, updateURL, projectid);
}





