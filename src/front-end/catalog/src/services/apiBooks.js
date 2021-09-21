import api from './api';

class ApiBooks {
    async getAll() {
        return await api.get('Books');
    }

    async getByCriteria(criteria, search) {
        return await api.get(`Books/${criteria},${search}`);
    }

    async getById(id) {
        return await api.get(`Books/${id}`);
    }

    async create(book) {
        return await api.post('Books/', book);
    }
    
    async delete(id) {
        return await api.delete(`Books/${id}`);
    }
    
    async update(book) {
        return await api.put(`Books/${book.id}`, book);
    }
}

export default new ApiBooks();