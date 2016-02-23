var AllQuestionsView = function (options) {
    var data = {};
    data.el = options.el;
    data.table = data.el.find("#questionTable");
    data.table.jqGrid({
        url: options.url,
        datatype: "json",
        height: "800px",
        colNames: ["Text", "Answer", "Distractors", "QuestionId"],
        colModel: [
            { name: "Text", index: "Text", searchoptions: { sopt: ["eq", "cn", "bw"] } },
            { name: "Answer", index: "Answer", searchoptions: { sopt: ["eq", "cn", "bw"] } },
            { name: "Distractors", index: "Distractors", searchoptions: { sopt: ["eq", "cn", "bw"] } },
            { name: "QuestionId", index: "QuestionId", hidden: true },
        ],
        rowNum: 30,
        rowList: [30, 50, 100],
        pager: "#pager",
        viewrecords: true,
        sortorder: "asc",
        autowidth: true,
        loadonce: true,
        onSelectRow: function (id) {
            var rowData = $(this).getLocalRow(id);
            window.location = "#question/" + rowData.QuestionId;
        }
    });
    
    data.table.jqGrid("filterToolbar", { searchOperators: true });


    return {
        show: function() {
            data.table.setGridParam({ datatype: "json", page: 1 }).trigger('reloadGrid');
            data.el.show();
        },
        hide: function() {
            data.el.hide();
        }
    };
};