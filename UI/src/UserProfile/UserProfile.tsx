import * as React from "react";

import { Spinner, Stack, StackItem } from "@fluentui/react";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import configData from "../config.json";
import { SupplierProfile } from "./SupplierProfile";
import { BuyerProfile } from "./BuyerProfile";

const UserProfile: React.FunctionComponent = () => {
  const [isSupplier, SetIsSupplier] = React.useState<boolean | undefined>();

  const { user } = useAuth0();

  React.useEffect(() => {
    SetIsSupplier(user[configData.roleIdentifier] === "Supplier");
  });

  if (isSupplier === undefined) {
    return (
      <Stack horizontalAlign={"center"} verticalAlign={"center"}>
        <Spinner />
      </Stack>
    );
  }

  return isSupplier === true ? <SupplierProfile /> : <BuyerProfile />;
};

export default withAuthenticationRequired(UserProfile, {
  onRedirecting: () => <Spinner />,
});
