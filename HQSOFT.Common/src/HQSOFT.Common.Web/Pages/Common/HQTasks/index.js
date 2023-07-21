$(function () {
    var l = abp.localization.getResource("Common");
	
	var hQTaskService = window.hQSOFT.common.hQTasks.hQTask;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQTasks/CreateModal",
        scriptUrl: "/Pages/Common/HQTasks/createModal.js",
        modalClass: "hQTaskCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQTasks/EditModal",
        scriptUrl: "/Pages/Common/HQTasks/editModal.js",
        modalClass: "hQTaskEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            subject: $("#SubjectFilter").val(),
			project: $("#ProjectFilter").val(),
			status: $("#StatusFilter").val(),
			priority: $("#PriorityFilter").val(),
			expectedStartDateMin: $("#ExpectedStartDateFilterMin").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			expectedStartDateMax: $("#ExpectedStartDateFilterMax").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			expectedEndDateMin: $("#ExpectedEndDateFilterMin").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			expectedEndDateMax: $("#ExpectedEndDateFilterMax").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			expectedTimeMin: $("#ExpectedTimeFilterMin").val(),
			expectedTimeMax: $("#ExpectedTimeFilterMax").val(),
			processMin: $("#ProcessFilterMin").val(),
			processMax: $("#ProcessFilterMax").val()
        };
    };

    var dataTable = $("#HQTasksTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(hQTaskService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.HQTasks.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.HQTasks.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    hQTaskService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "subject" },
			{ data: "project" },
            {
                data: "status",
                render: function (status) {
                    if (status === undefined ||
                        status === null) {
                        return "";
                    }

                    var localizationKey = "Enum:StatusTask." + status;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "priority",
                render: function (priority) {
                    if (priority === undefined ||
                        priority === null) {
                        return "";
                    }

                    var localizationKey = "Enum:PriorityTask." + priority;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
            {
                data: "expectedStartDate",
                render: function (expectedStartDate) {
                    if (!expectedStartDate) {
                        return "";
                    }
                    
					var date = Date.parse(expectedStartDate);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                data: "expectedEndDate",
                render: function (expectedEndDate) {
                    if (!expectedEndDate) {
                        return "";
                    }
                    
					var date = Date.parse(expectedEndDate);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
			{ data: "expectedTime" },
			{ data: "process" }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewHQTaskButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        hQTaskService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/common/h-qTasks/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'subject', value: input.subject }, 
                            { name: 'project', value: input.project }, 
                            { name: 'status', value: input.status }, 
                            { name: 'priority', value: input.priority },
                            { name: 'expectedStartDateMin', value: input.expectedStartDateMin },
                            { name: 'expectedStartDateMax', value: input.expectedStartDateMax },
                            { name: 'expectedEndDateMin', value: input.expectedEndDateMin },
                            { name: 'expectedEndDateMax', value: input.expectedEndDateMax },
                            { name: 'expectedTimeMin', value: input.expectedTimeMin },
                            { name: 'expectedTimeMax', value: input.expectedTimeMax },
                            { name: 'processMin', value: input.processMin },
                            { name: 'processMax', value: input.processMax }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reload();
    });
    
    
});
