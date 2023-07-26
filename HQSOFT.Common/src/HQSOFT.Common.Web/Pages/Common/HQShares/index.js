$(function () {
    var l = abp.localization.getResource("Common");
	
	var hQShareService = window.hQSOFT.common.hQShares.hQShare;
	
        var lastNpIdId = '';
        var lastNpDisplayNameId = '';

        var _lookupModal = new abp.ModalManager({
            viewUrl: abp.appPath + "Shared/LookupModal",
            scriptUrl: "/Pages/Shared/lookupModal.js",
            modalClass: "navigationPropertyLookup"
        });

        $('.lookupCleanButton').on('click', '', function () {
            $(this).parent().find('input').val('');
        });

        _lookupModal.onClose(function () {
            var modal = $(_lookupModal.getModal());
            $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
            $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
        });
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQShares/CreateModal",
        scriptUrl: "/Pages/Common/HQShares/createModal.js",
        modalClass: "hQShareCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Common/HQShares/EditModal",
        scriptUrl: "/Pages/Common/HQShares/editModal.js",
        modalClass: "hQShareEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            iDParent: $("#IDParentFilter").val(),
            canRead: (function () {
                var value = $("#CanReadFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canWrite: (function () {
                var value = $("#CanWriteFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canSubmit: (function () {
                var value = $("#CanSubmitFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            canShare: (function () {
                var value = $("#CanShareFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
			identityUserId: $("#IdentityUserIdFilter").val()
        };
    };

    var dataTable = $("#HQSharesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(hQShareService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Common.HQShares.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.hQShare.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Common.HQShares.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    hQShareService.delete(data.record.hQShare.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "hqShare.iDParent" },
            {
                data: "hqShare.canRead",
                render: function (canRead) {
                    return canRead ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "hqShare.canWrite",
                render: function (canWrite) {
                    return canWrite ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "hqShare.canSubmit",
                render: function (canSubmit) {
                    return canSubmit ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "hqShare.canShare",
                render: function (canShare) {
                    return canShare ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            },
            {
                data: "identityUser.email",
                defaultContent : ""
            }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewHQShareButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        hQShareService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/common/h-qShares/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'iDParent', value: input.iDParent }, 
                            { name: 'canRead', value: input.canRead }, 
                            { name: 'canWrite', value: input.canWrite }, 
                            { name: 'canSubmit', value: input.canSubmit }, 
                            { name: 'canShare', value: input.canShare }, 
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
            dataTable.ajax.reloadEx();;
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();;
    });
    
    
});
