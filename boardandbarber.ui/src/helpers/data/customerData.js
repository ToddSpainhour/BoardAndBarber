import Axios from "axios";
import {baseUrl} from "../constants.json"

const getAllCustomers = () => new Promise((resolve,reject) => {

  Axios.get(`${baseUrl}/Customers`)
  .then(response => {
    let customers = response.data;
    resolve(customers);
  })
  .catch(reject);     
});

export default { getAllCustomers };
