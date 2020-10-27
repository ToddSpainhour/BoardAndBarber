import './App.scss';
import '../index';
import '../components/shared/SingleCustomer/SingleCustomer'
import SingleCustomer from '../components/shared/SingleCustomer/SingleCustomer';

function App() {
  return (
    <div className="App">
      <p>Hello there.</p>
      <SingleCustomer id="1" name="Todd" birthday="1/2/1234" favoriteBarber="Jimbo" notes="no talking"/>
    </div>
  );
}

export default App;
