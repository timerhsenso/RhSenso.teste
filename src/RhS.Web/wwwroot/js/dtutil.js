
// DTUtil v4 — utilitário para montar DataTables padrão da aplicação
// Requer: jQuery, DataTables, Bootstrap (para estilos).

window.DTUtil = (function () {
  function init(opts) {
    const $tbl = $("#" + opts.tableId);
    const $card = $tbl.closest(".card");
    const $btnCsv = $card.find("[data-dtbtn='export-csv']");
    const $btnXlsx = $card.find("[data-dtbtn='export-excel']");
    const $btnPdf = $card.find("[data-dtbtn='export-pdf']");
    const $btnCols = $card.find("[data-dtbtn='toggle-cols']");
    const $btnBulk = $card.find("[data-dtbtn='bulk-delete']");
    const $btnFilter = $card.find("[data-dtbtn='open-filter']");

    if (!opts.hideExports) { $btnCsv.removeClass("d-none"); $btnXlsx.removeClass("d-none"); $btnPdf.removeClass("d-none"); }
    if (opts.selectable) $btnBulk.removeClass("d-none");
    if (!opts.hideLength || !opts.hideSearch) $btnCols.removeClass("d-none");

    const dtCols = [];
    if (opts.selectable) {
      dtCols.push({ data: null, orderable: false, className: "text-center", width: "32px",
        render: (_, __, row) => `<input type="checkbox" class="dt-row-select" value="${row.id}" />` });
    }
    dtCols.push(...opts.columns);
    if (opts.actions) {
      dtCols.push({ data: null, orderable: false, className: "text-end",
        render: (_, __, row) => {
          const tpl = document.getElementById(`${opts.tableId}-actions-tpl`)?.innerHTML ?? "";
          return tpl.replaceAll("{{id}}", row.id);
        }});
    }

    const dt = $tbl.DataTable({
      processing: true,
      serverSide: (opts.serverSide ?? true),
      searching: !(opts.hideSearch ?? true),
      lengthChange: !(opts.hideLength ?? true),
      ajax: {
        url: opts.ajaxUrl,
        type: "POST",
        data: function (d) {
          d.Draw = d.draw; d.Start = d.start; d.Length = d.length;
          if (d.order?.length) {
            const first = d.order[0]; const col = d.columns[first.column].data;
            d.SortBy = typeof col === "string" ? col : null; d.SortDir = first.dir;
          }
          d.Search = d.search?.value ?? null;
          d.FilterJson = opts.filterBuilder ? JSON.stringify(opts.filterBuilder.getFilter()) : null;
          if (typeof opts.extraPostData === "function") Object.assign(d, opts.extraPostData());
        }
      },
      columns: dtCols
    });

    $tbl.on("click", "[data-act='edit']", function () { const id = this.getAttribute("data-id"); opts.onEdit && opts.onEdit(id); });
    $tbl.on("click", "[data-act='delete']", function () { const id = this.getAttribute("data-id"); opts.onDelete && opts.onDelete(id, () => dt.ajax.reload(null, false)); });

    $btnBulk.on("click", function () {
      const ids = [...$tbl[0].querySelectorAll(".dt-row-select:checked")].map(x => x.value);
      if (!ids.length) return;
      opts.onBulkDelete && opts.onBulkDelete(ids, () => dt.ajax.reload(null, false));
    });

    $btnCsv.on("click", () => postFile(opts.endpoints?.exportCsv, opts.filterBuilder));
    $btnXlsx.on("click", () => postFile(opts.endpoints?.exportExcel, opts.filterBuilder));
    $btnPdf.on("click", () => postFile(opts.endpoints?.exportPdf, opts.filterBuilder));

    function postFile(url, fb) {
      if (!url) return;
      const filterJson = fb ? JSON.stringify(fb.getFilter()) : null;
      fetch(url, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(filterJson) })
        .then(r => r.blob()).then(b => { const a = document.createElement("a"); a.href = URL.createObjectURL(b); a.download = "export"; a.click(); setTimeout(() => URL.revokeObjectURL(a.href), 2000); });
    }

    $btnCols.on("click", () => {
      const total = dt.columns().count();
      const idx = prompt(`Índice da coluna para alternar visibilidade (0 a ${total - 1})`);
      if (idx == null) return;
      const i = parseInt(idx, 10);
      if (!isNaN(i) && i >= 0 && i < total) { const col = dt.column(i); col.visible(!col.visible()); }
    });

    $btnFilter.on("click", () => { opts.filterBuilder?.open(); });

    return { reload: () => dt.ajax.reload(null, false), table: dt };
  }
  return { init };
})();
