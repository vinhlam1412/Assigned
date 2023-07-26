var abp = abp || {};

abp.modals.hQShareCreate = function () {
    var initModal = function (publicApi, args) {
        var l = abp.localization.getResource("Common");
        
        
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
        
        publicApi.onOpen(function () {
            $('#HQShare_IdentityUserId').select2({
                dropdownParent: $('#HQShareCreateModal'),
                ajax: {
                    url: abp.appPath + 'api/common/h-qShares/identity-user-lookup',
                    type: 'GET',
                    data: function (params) {
                        return { filter: params.term, maxResultCount: 10 }
                    },
                    processResults: function (data) {
                        var mappedItems = _.map(data.items, function (item) {
                            return { id: item.id, text: item.displayName };
                        });

                        mappedItems.unshift({ id: '', text: ' - ' });
                        return { results: mappedItems };
                    }
                }
            });
        });

        
    };

    return {
        initModal: initModal
    };
};
