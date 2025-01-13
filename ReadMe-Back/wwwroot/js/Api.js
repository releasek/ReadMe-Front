
window.Api = {
    // 取得年度銷售總額
    async fetchAmount(year) {
        const response = await fetch(`/api/OrderApi/totalAmount?year=${year}`, {
            method: "GET",
        });
        if (!response.ok) {
            throw new Error(`Failed to fetch total amount: ${response.statusText}`);
        }
        return await response.json(); // 取得年度總銷售金額
    },

    // 取得年度銷售數量
    async fetchQuantity(year) {
        const response = await fetch(`/api/OrderApi/totalQuantity?year=${year}`, {
            method: "GET",
        });
        if (!response.ok) {
            throw new Error(`Failed to fetch total quantity: ${response.statusText}`);
        }
        return await response.json(); // 取得年度總銷售數量
    },

    // 取得每季的銷售數據
    async fetchQuarterlyData(year) {
        const response = await fetch(`/api/OrderApi/quarterlyData?year=${year}`, {
            method: "GET",
        });
        if (!response.ok) {
            throw new Error(`Failed to fetch quarterly data: ${response.statusText}`);
        }
        return await response.json(); // 取得每季銷售數據
    },
};
