import './App.scss';
import '../index';
import '../components/shared/SingleCustomer/SingleCustomer'
import Customers from '../components/pages/Customer/Customer';

function App() {
  return (
    <div className="App">
      <p>Hello there.</p>
      <Customers />
    </div>
  );
}

export default App;
