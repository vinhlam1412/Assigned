$(function () {
    var l = abp.localization.getResource("Common");
	
	var hQNotificationService = window.hQSOFT.common.hQNotifications.hQNotification;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQNotifications/CreateModal",
        scriptUrl: "/Pages/Common/HQNotifications/createModal.js",
        modalClass: "hQNotificationCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQNotifications/EditModal",
        scriptUrl: "/Pages/Common/HQNotifications/editModal.js",
        modalClass: "hQNotificationEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            iDParent: $("#IDParentFilter").val(),
			toUser: $("#ToUserFilter").val(),
			fromUser: $("#FromUserFilter").val(),
			notiTitle: $("#NotiTitleFilter").val(),
			notiBody: $("#NotiBodyFilter").val(),
			type: $("#TypeFilter").val(),
            isRead: (function () {
                var value = $("#isReadFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })()
        };
    };

    var dataTable = $("#HQNotificationsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(hQNotificationService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.HQNotifications.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.HQNotifications.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    hQNotificationService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "iDParent" },
			{ data: "toUser" },
			{ data: "fromUser" },
			{ data: "notiTitle" },
			{ data: "notiBody" },
			{ data: "type" },
            {
                data: "isRead",
                render: function (isRead) {
                    return isRead ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewHQNotificationButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        hQNotificationService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/common/h-qNotifications/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'iDParent', value: input.iDParent }, 
                            { name: 'toUser', value: input.toUser }, 
                            { name: 'fromUser', value: input.fromUser }, 
                            { name: 'notiTitle', value: input.notiTitle }, 
                            { name: 'notiBody', value: input.notiBody }, 
                            { name: 'type', value: input.type }, 
                            { name: 'isRead', value: input.isRead }
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
            dataTable.ajax.reloadEx();;
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();;
    });
    
    
});
