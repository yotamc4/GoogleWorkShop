import * as React from "react";
import * as SuppliersControllerService from "../Services/SuppliersControllerService";

import { Pivot, PivotItem } from "office-ui-fabric-react/lib/Pivot";
import { buttonHeaderProps } from "./UserProfileStyles";
import { Spinner, Stack } from "@fluentui/react";
import { GroupsList } from "./GroupsList";
import { Bid } from "../Modal/GroupDetails";
import { useAuth0 } from "@auth0/auth0-react";
import ButtonAppBar from "../LoginBar";

export const SupplierProfile: React.FunctionComponent = () => {
  const [groupsWon, setGroupsWon] = React.useState<Bid[]>();
  const [potentialGroupsToSupply, setPotentialGroupsToSupply] = React.useState<
    Bid[]
  >();

  const { getAccessTokenSilently } = useAuth0();

  async function updateCurrentGroups() {
    let [potentialGroupsToSupply, groupsWon] = await Promise.all([
      SuppliersControllerService.GetGroupsSupplierIsParticipant(
        getAccessTokenSilently
      ),

      SuppliersControllerService.GetGroupsSupplierHasWon(
        getAccessTokenSilently
      ),
    ]);

    setGroupsWon(groupsWon.bidsPage);
    setPotentialGroupsToSupply(potentialGroupsToSupply.bidsPage);
  }

  React.useEffect(() => {
    updateCurrentGroups();
  }, []);

  return (
    <Stack horizontalAlign="center">
      <ButtonAppBar />
      <Pivot styles={{ root: { marginBottom: "2rem" } }}>
        <PivotItem
          headerButtonProps={{
            text: "Groups won",
            styles: buttonHeaderProps,
          }}
        >
          {groupsWon ? <GroupsList groups={groupsWon} /> : <Spinner />}
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Active groups",
            styles: buttonHeaderProps,
          }}
        >
          {potentialGroupsToSupply ? (
            <GroupsList groups={potentialGroupsToSupply} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
      </Pivot>
    </Stack>
  );
};
