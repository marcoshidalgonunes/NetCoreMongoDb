import api from "./api";

class ApiBooks {
    async getAll() {
        return await api.get("Books");
    }
}

export default new ApiBooks();