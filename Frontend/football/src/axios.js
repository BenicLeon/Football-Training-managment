import axios from 'axios';

const instance = axios.create({
  baseURL: 'http://localhost:7070', 
});

export default instance;