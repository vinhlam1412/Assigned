$(function () {
    var l = abp.localization.getResource("Common");
	
	var hQAssignedService = window.hQSOFT.common.hQAssigneds.hQAssigned;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQAssigneds/CreateModal",
        scriptUrl: "/Pages/Common/HQAssigneds/createModal.js",
        modalClass: "hQAssignedCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQAssigneds/EditModal",
        scriptUrl: "/Pages/Common/HQAssigneds/editModal.js",
        modalClass: "hQAssignedEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            iDParent: $("#IDParentFilter").val(),
			completebyMin: $("#CompletebyFilterMin").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			completebyMax: $("#CompletebyFilterMax").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			priority: $("#PriorityFilter").val(),
			comment: $("#CommentFilter").val(),
			identityUserId: $("#IdentityUserFilter").val()
        };
    };

    var dataTable = $("#HQAssignedsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(hQAssignedService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.HQAssigneds.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.hQAssigned.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.HQAssigneds.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    hQAssignedService.delete(data.record.hQAssigned.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "hqAssigned.iDParent" },
            {
                data: "hqAssigned.completeby",
                render: function (completeby) {
                    if (!completeby) {
                        return "";
                    }
                    
					var date = Date.parse(completeby);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                data: "hqAssigned.priority",
                render: function (priority) {
                    if (priority === undefined ||
                        priority === null) {
                        return "";
                    }

                    var localizationKey = "Enum:PriorityAssign." + priority;
                    var localized = l(localizationKey);

                    if (localized === localizationKey) {
                        abp.log.warn("No localization found for " + localizationKey);
                        return "";
                    }

                    return localized;
                }
            },
			{ data: "hqAssigned.comment" }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewHQAssignedButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        hQAssignedService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/common/h-qAssigneds/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'iDParent', value: input.iDParent },
                            { name: 'completebyMin', value: input.completebyMin },
                            { name: 'completebyMax', value: input.completebyMax }, 
                            { name: 'priority', value: input.priority }, 
                            { name: 'comment', value: input.comment }, 
                            { name: 'identityUserId', value: input.identityUserId }
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
    
                $('#IdentityUserFilter').select2({
                ajax: {
                    url: abp.appPath + 'api/common/h-qAssigneds/identity-user-lookup',
                    type: 'GET',
                    data: function (params) {
                        return { filter: params.term, maxResultCount: 10 }
                    },
                    processResults: function (data) {
                        var mappedItems = _.map(data.items, function (item) {
                            return { id: item.id, text: item.displayName };
                        });
                        mappedItems.unshift({ id: "", text: ' - ' });

                        return { results: mappedItems };
                    }
                }
            });
        
});
