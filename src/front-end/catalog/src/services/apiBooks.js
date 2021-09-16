import api from './api';

class ApiBooks {
    async getAll() {
        return await api.get('Books');
    }
    async getByCriteria(criteria, search) {
        return await api.get(`Books/${criteria},${search}`);
    }
}

export default new ApiBooks();