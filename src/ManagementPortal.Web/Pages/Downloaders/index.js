$(function () {
    var l = abp.localization.getResource('ManagementPortal');

    var downloaderService = window.managementPortal.downloaders.downloaders;

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Downloaders/CreateModal',
        scriptUrl: abp.appPath + 'Pages/Downloaders/createModal.js',
        modalClass: 'downloaderCreate',
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Downloaders/EditModal',
        scriptUrl: abp.appPath + 'Pages/Downloaders/editModal.js',
        modalClass: 'downloaderEdit',
    });

    var getFilter = function () {
        return {
            filterText: $('#FilterText').val(),
            downloaderEnabled: (function () {
                var value = $('#DownloaderEnabledFilter').val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            downloaderPollarName: $('#DownloaderPollarNameFilter').val(),
        };
    };

    var dataTableColumns = [
        {
            rowAction: {
                items: [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('ManagementPortal.Downloaders.Edit'),
                        action: function (data) {
                            editModal.open({
                                id: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('ManagementPortal.Downloaders.Delete'),
                        confirmMessage: function () {
                            return l('DeleteConfirmationMessage');
                        },
                        action: function (data) {
                            downloaderService.delete(data.record.id).then(function () {
                                abp.notify.success(l('SuccessfullyDeleted'));
                                dataTable.ajax.reloadEx();
                            });
                        },
                    },
                ],
            },
        },
        {
            data: 'downloaderEnabled',

            render: function (downloaderEnabled) {
                return downloaderEnabled
                    ? '<i class="fa fa-check"></i>'
                    : '<i class="fa fa-times"></i>';
            },
        },
        { data: 'downloaderPollarName' },
    ];

    var showDetailRows = abp.auth.isGranted('ManagementPortal.DownloaderWebSockets');
    if (showDetailRows) {
        dataTableColumns.unshift({
            class: 'details-control text-center',
            orderable: false,
            data: null,
            defaultContent: '<i class="fa fa-chevron-down"></i>',
            width: '0.1rem',
        });
    } else {
        $('#DetailRowTHeader').remove();
    }

    if (abp.auth.isGranted('ManagementPortal.Downloaders.Delete')) {
        dataTableColumns.unshift({
            targets: 0,
            data: null,
            orderable: false,
            className: 'select-checkbox',
            width: '0.5rem',
            render: function (data) {
                return (
                    '<input type="checkbox" class="form-check-input select-row-checkbox" data-id="' +
                    data.id +
                    '"/>'
                );
            },
        });
    } else {
        $('#BulkDeleteCheckboxTheader').remove();
    }

    var dataTable = $('#DownloadersTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            responsive: true,
            order: [[3, 'desc']],
            ajax: abp.libs.datatables.createAjax(downloaderService.getList, getFilter),
            columnDefs: dataTableColumns,
        })
    );

    dataTable.on('xhr', function () {
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
        $('#select_all').prop('indeterminate', false);
        $('#select_all').prop('checked', false);
    });

    function selectOrUnselectAllCheckboxes(selectAll) {
        $('.select-row-checkbox').each(function () {
            $(this).prop('checked', selectAll);
        });
    }

    $('#select_all').click(function () {
        if ($(this).is(':checked')) {
            selectOrUnselectAllCheckboxes(true);
        } else {
            $('.select-row-checkbox').each(function () {
                selectOrUnselectAllCheckboxes(false);
            });
        }

        showOrHideContextMenu();
    });

    dataTable.on('change', "input[type='checkbox'].select-row-checkbox", function () {
        var unSelectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:not(:checked)");

        if (unSelectedCheckboxes.length >= 1) {
            var dataRecordTotal = dataTable.context[0].json.data.length;
            if (unSelectedCheckboxes.length === dataRecordTotal) {
                $('#select_all').prop('indeterminate', false);
                $('#select_all').prop('checked', false);
            } else {
                $('#select_all').prop('indeterminate', true);
            }
        } else {
            $('#select_all').prop('indeterminate', false);
            $('#select_all').prop('checked', true);
        }

        showOrHideContextMenu();
    });

    var showOrHideContextMenu = function () {
        var selectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:is(:checked)");
        var selectedCheckboxCount = selectedCheckboxes.length;
        var dataRecordTotal = dataTable.context[0].json.data.length;
        var recordsTotal = dataTable.context[0].json.recordsTotal;

        if (selectedCheckboxCount >= 1) {
            $('#bulk-delete-context-menu').removeClass('d-none');

            $('#items-selected-info-message').html(
                selectedCheckboxCount === 1
                    ? l('OneItemOnThisPageIsSelected')
                    : l('NumberOfItemsOnThisPageAreSelected', selectedCheckboxCount)
            );

            $('#items-selected-info-message').removeClass('d-none');

            if (selectedCheckboxCount === dataRecordTotal && recordsTotal > dataRecordTotal) {
                $('#select-all-items-btn').html(l('SelectAllItems', recordsTotal));
                $('#select-all-items-btn').removeClass('d-none');

                $('#select-all-items-btn').off('click');
                $('#select-all-items-btn').click(function () {
                    $(this).data('selected', true);
                    $(this).addClass('d-none');
                    $('#items-selected-info-message').html(l('AllItemsAreSelected', recordsTotal));
                    $('#clear-selection-btn').removeClass('d-none');
                });

                $('#clear-selection-btn').off('click');
                $('#clear-selection-btn').click(function () {
                    $('#select-all-items-btn').data('selected', false);
                    $('#select_all').prop('checked', false);
                    selectOrUnselectAllCheckboxes(false);
                    showOrHideContextMenu();
                });
            } else {
                $('#select-all-items-btn').addClass('d-none');
                $('#select-all-items-btn').data('selected', false);
                $('#clear-selection-btn').addClass('d-none');
            }

            $('#delete-selected-items').off('click');
            $('#delete-selected-items').click(function () {
                if ($('#select-all-items-btn').data('selected') === true) {
                    abp.message.confirm(l('DeleteAllRecords'), function (confirmed) {
                        if (!confirmed) {
                            return;
                        }

                        downloaderService.deleteAll(getFilter()).then(function () {
                            dataTable.ajax.reloadEx();
                            selectOrUnselectAllCheckboxes(false);
                            showOrHideContextMenu();
                        });
                    });
                } else {
                    var selectedCheckboxes = $(
                        "input[type='checkbox'].select-row-checkbox:is(:checked)"
                    );
                    var selectedRecordsIds = [];

                    for (var i = 0; i < selectedCheckboxes.length; i++) {
                        selectedRecordsIds.push($(selectedCheckboxes[i]).data('id'));
                    }

                    abp.message.confirm(
                        l('DeleteSelectedRecords', selectedCheckboxes.length),
                        function (confirmed) {
                            if (!confirmed) {
                                return;
                            }

                            downloaderService.deleteByIds(selectedRecordsIds).then(function () {
                                dataTable.ajax.reloadEx();
                                selectOrUnselectAllCheckboxes(false);
                                showOrHideContextMenu();
                            });
                        }
                    );
                }
            });
        } else {
            $('#bulk-delete-context-menu').addClass('d-none');
            $('#select-all-items-btn').addClass('d-none');
            $('#items-selected-info-message').addClass('d-none');
            $('#clear-selection-btn').addClass('d-none');
        }
    };

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#NewDownloaderButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#SaveMaxWorkerButton').click(function (e) {
        e.preventDefault();
        var value = parseInt($('#MaxWorkerInput').val(), 10);
        if (isNaN(value) || value < 1) {
            abp.notify.warn(l('InvalidValue'));
            return;
        }
        downloaderService.setMaxWorker(value).then(function () {
            $('#MaxWorkerSaveStatus').fadeIn().delay(2000).fadeOut();
        });
    });

    $('#SearchForm').submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#ExportToExcelButton').click(function (e) {
        e.preventDefault();

        downloaderService.getDownloadToken().then(function (result) {
            var input = getFilter();
            var url =
                abp.appPath +
                'api/app/downloaders/as-excel-file' +
                abp.utils.buildQueryString([
                    { name: 'downloadToken', value: result.token },
                    { name: 'filterText', value: input.filterText },
                    { name: 'downloaderEnabled', value: input.downloaderEnabled },
                    { name: 'downloaderPollarName', value: input.downloaderPollarName },
                ]);

            var downloadWindow = window.open(url, '_blank');
            downloadWindow.focus();
        });
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
        var iconCss = $('#AdvancedFilterSection').is(':visible')
            ? 'fa ms-1 fa-angle-up'
            : 'fa ms-1 fa-angle-down';
        $(this).find('i').attr('class', iconCss);
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();
            selectOrUnselectAllCheckboxes(false);
            showOrHideContextMenu();
        }
    });

    $('#AdvancedFilterSection select').change(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#DownloadersTable').on('click', 'td.details-control', function () {
        $(this).find('i').toggleClass('fa-chevron-down').toggleClass('fa-chevron-up');

        var tr = $(this).parents('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            var data = row.data();

            detailRows(data).done(function (result) {
                row.child(result).show();
                initDataGrids(data);
            });

            tr.addClass('shown');
        }
    });

    function detailRows(data) {
        return $.ajax(abp.appPath + 'Downloaders/ChildDataGrid?downloaderId=' + data.id).done(
            function (result) {
                return result;
            }
        );
    }

    function initDataGrids(data) {
        initDownloaderWebSocketGrid(data);
    }

    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>

    function initDownloaderWebSocketGrid(data) {
        if (!abp.auth.isGranted('ManagementPortal.DownloaderWebSockets')) {
            return;
        }

        var downloaderId = data.id;

        var downloaderWebSocketService =
            window.managementPortal.downloaderWebSockets.downloaderWebSockets;

        var downloaderWebSocketCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'DownloaderWebSockets/CreateModal',
            scriptUrl: abp.appPath + 'Pages/DownloaderWebSockets/createModal.js',
            modalClass: 'downloaderWebSocketCreate',
        });

        var downloaderWebSocketEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'DownloaderWebSockets/EditModal',
            scriptUrl: abp.appPath + 'Pages/DownloaderWebSockets/editModal.js',
            modalClass: 'downloaderWebSocketEdit',
        });

        var downloaderWebSocketDataTable = $(
            '#DownloaderWebSocketsTable-' + downloaderId
        ).DataTable(
            abp.libs.datatables.normalizeConfiguration({
                processing: true,
                serverSide: true,
                paging: true,
                searching: false,
                responsive: true,
                scrollX: true,
                autoWidth: true,
                scrollCollapse: true,
                order: [[1, 'asc']],
                ajax: abp.libs.datatables.createAjax(
                    downloaderWebSocketService.getListByDownloaderId,
                    {
                        downloaderId: downloaderId,
                        maxResultCount: 5,
                    }
                ),
                columnDefs: [
                    {
                        rowAction: {
                            items: [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted(
                                        'ManagementPortal.DownloaderWebSockets.Edit'
                                    ),
                                    action: function (data) {
                                        downloaderWebSocketEditModal.open({
                                            id: data.record.id,
                                        });
                                    },
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted(
                                        'ManagementPortal.DownloaderWebSockets.Delete'
                                    ),
                                    confirmMessage: function () {
                                        return l('DeleteConfirmationMessage');
                                    },
                                    action: function (data) {
                                        downloaderWebSocketService
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('SuccessfullyDeleted'));
                                                downloaderWebSocketDataTable.ajax.reloadEx();
                                            });
                                    },
                                },
                            ],
                        },
                        width: '1rem',
                    },
                    { data: 'host', width: '0.1rem' },
                    { data: 'port', width: '0.1rem' },
                ],
            })
        );

        downloaderWebSocketCreateModal.onResult(function () {
            downloaderWebSocketDataTable.ajax.reloadEx();
        });

        downloaderWebSocketEditModal.onResult(function () {
            downloaderWebSocketDataTable.ajax.reloadEx();
        });

        $('button.NewDownloaderWebSocketButton')
            .off('click')
            .on('click', function (e) {
                downloaderWebSocketCreateModal.open({
                    downloaderId: $(this).data('downloader-id'),
                });
            });
    }
});

