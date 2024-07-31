import { Book } from '../types/Book';
import Api from './Api';

const apiName: string = '/Books';

class BookApi extends Api {
    async getAll(): Promise<Book[]> {
        let books: Book[] = [];
        await this.api.get(apiName)
            .then(response => books = response.data);
        return books;
    }  

    async getByCriteria(criteria: string, search: string): Promise<Book[]> {
        let books: Book[] = [];
        await this.api.get(`${apiName}/${criteria}/${search}`)
            .then(response => books = response.data);
        return books;
    }

    async getById(id: string): Promise<Book> {
        // eslint-disable-next-line @typescript-eslint/consistent-type-assertions
        let book: Book = <Book>{};
        await this.api.get(`${apiName}/${id}`)
          .then(response => book = response.data);
        return book;
    }

    async create(book: Book): Promise<Book> {
        return await this.api.post(apiName, book);
    }
    
    async update(book: Book) {
        return await this.api.put(apiName, book);
    }
    
    async delete(id: string) {
        return await this.api.delete(`${apiName}/${id}`);
    }
}

// eslint-disable-next-line import/no-anonymous-default-export
export default new BookApi();
