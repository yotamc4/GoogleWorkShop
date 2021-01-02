import * as React from "react";
import * as BuyersControllerServices from "../Services/BuyersControllerServices";

import { Pivot, PivotItem } from "office-ui-fabric-react/lib/Pivot";
import { buttonHeaderProps } from "./UserProfileStyles";
import { Spinner, Stack } from "@fluentui/react";
import { GroupsList } from "./GroupsList";
import { Bid } from "../Modal/GroupDetails";

export const UserProfile: React.FunctionComponent = () => {
  const [groupsUserMemberIn, setGroupsUserMemberIn] = React.useState<Bid[]>();
  const [groupsCreatedByTheUser, setGroupsCreatedByTheUser] = React.useState<
    Bid[]
  >();

  // Replace with real user Id
  async function updateCurrentProductAndPageNumber() {
    //TODO - Replace "1" with real userId
    let [groupsCreatedByTheUser, groupsUserMemberIn] = await Promise.all([
      BuyersControllerServices.GetBidsCreatedByBuyer(/* useId */ "1"),
      BuyersControllerServices.GetGroupsBuyerIsParticipant(/* useId */ "1"),
    ]);

    setGroupsCreatedByTheUser(groupsCreatedByTheUser.bidsPage);
    setGroupsUserMemberIn(groupsUserMemberIn.bidsPage);
  }

  React.useEffect(() => {
    updateCurrentProductAndPageNumber();
  }, []);

  return (
    <Stack horizontalAlign="center">
      <Pivot styles={{ root: { marginBottom: "2rem" } }}>
        <PivotItem
          headerButtonProps={{
            text: "Groups I was sign in into",
            styles: buttonHeaderProps,
          }}
        >
          {groupsUserMemberIn ? (
            <GroupsList groups={groupsUserMemberIn} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Groups created by me",
            styles: buttonHeaderProps,
          }}
        >
          {groupsCreatedByTheUser ? (
            <GroupsList groups={groupsCreatedByTheUser} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Groups I'm interested in",
            styles: buttonHeaderProps,
          }}
        >
          {groupsUserMemberIn ? (
            <GroupsList groups={groupsUserMemberIn} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
      </Pivot>
    </Stack>
  );
};
